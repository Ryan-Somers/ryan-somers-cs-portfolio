using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using rsH60Customer.DTO;
using rsH60Customer.Models;
using rsH60Customer.Models.Interfaces;

public class ShoppingCartController : Controller
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomersRepository _customerRepository;
    private readonly ISoapServiceRepository _soapClient;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly INotyfService _notyf;

    public ShoppingCartController(
        IShoppingCartRepository shoppingCartRepository,
        ICartItemRepository cartItemRepository,
        IProductRepository productRepository,
        ICustomersRepository customerRepository,
        IOrderRepository orderRepository,
        ISoapServiceRepository soapClient,
        IOrderItemRepository orderItemRepository,
        INotyfService notyf)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _soapClient = soapClient;
        _orderRepository = orderRepository;
        _notyf = notyf;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int cartId)
    {
        if (cartId <= 0)
        {
            return BadRequest("Invalid Cart ID.");
        }

        var cart = await _shoppingCartRepository.GetShoppingCartByIdAsync(cartId);
        if (cart == null)
        {
            return NotFound("Shopping cart not found.");
        }

        return View(cart.CartItems);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        if (quantity < 1)
        {
            return BadRequest("Quantity must be at least 1.");
        }

        // Fetch the cart item from the API
        var cartItem = await _cartItemRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem == null)
        {
            return NotFound("Cart item not found.");
        }

        // Update the quantity
        cartItem.Quantity = quantity;
        cartItem.Total = cartItem.Quantity * cartItem.Price;

        // Call the API to update the cart item
        await _cartItemRepository.UpdateCartItemAsync(cartItemId, cartItem);
        _notyf.Success("Updated Cart!");

        // Redirect back to the shopping cart view
        return RedirectToAction("Index", new { cartId = cartItem.CartId });
    }


    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not logged in.");
        }

        // Step 1: Check if cartId exists in claims
        var cartIdClaim = User.FindFirst("cartId")?.Value;
        int? cartId = string.IsNullOrEmpty(cartIdClaim) ? null : int.Parse(cartIdClaim);

        ShoppingCartDto? cartDto = null;

        if (cartId.HasValue)
        {
            // Attempt to retrieve the cart using the cartId from claims
            cartDto = await _shoppingCartRepository.GetShoppingCartByIdAsync(cartId.Value);

            if (cartDto == null)
            {
                // If cartId in claims is invalid (cart was deleted), remove it
                await RemoveCartIdClaimAsync();
            }
        }

        if (cartDto == null)
        {
            // If cartDto is still null, create a new shopping cart
            var customerId = await _shoppingCartRepository.GetCustomerIdByUserIdAsync(userId);
            if (!customerId.HasValue)
            {
                TempData["ErrorMessage"] = "Could not find customer for the logged-in user.";
                return RedirectToAction("Index", "Products");
            }

            cartDto = await _shoppingCartRepository.AddShoppingCartAsync(new ShoppingCartDto
            {
                CustomerId = customerId.Value,
                UserId = userId,
                DateCreated = DateTime.UtcNow,
                CartItems = new List<CartItemDto>()
            });

            if (cartDto == null)
            {
                TempData["ErrorMessage"] = "Unable to create a new shopping cart.";
                return RedirectToAction("Index", "Products");
            }

            // Update claims with the new cartId
            await UpdateCartIdClaimAsync(cartDto.CartId);
        }

        // Step 2: Fetch product details
        var product = await _productRepository.GetProductByIdAsync(productId);
        if (product == null)
        {
            TempData["ErrorMessage"] = "Product not found.";
            return RedirectToAction("Index", "Products");
        }

        // Step 3: Add or update the cart item
        var existingCartItem = cartDto.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (existingCartItem != null)
        {
            // Update the existing cart item's quantity and total
            existingCartItem.Quantity += quantity;
            existingCartItem.Total = existingCartItem.Quantity * product.SellPrice;

            await _cartItemRepository.UpdateCartItemAsync(existingCartItem.CartItemId, existingCartItem);
        }
        else
        {
            // Add a new cart item
            var cartItemDto = new CartItemDto
            {
                CartId = cartDto.CartId,
                ProductId = productId,
                ProductName = product.Description,
                Price = product.SellPrice,
                Quantity = quantity,
                Total = product.SellPrice * quantity
            };

            var addedItem = await _cartItemRepository.AddCartItemAsync(cartItemDto);
            if (addedItem == null)
            {
                TempData["ErrorMessage"] = "Failed to add the product to the cart.";
                return RedirectToAction("Index", "Products");
            }
        }

        // Step 4: Success
        TempData["SuccessMessage"] = $"{product.Description} added to cart successfully!";
        return RedirectToAction("Index", "Products");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        // Fetch the CartItem to get the CartId
        var cartItem = await _cartItemRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem == null)
        {
            return NotFound("Cart item not found.");
        }

        // Remove the item
        await _cartItemRepository.DeleteCartItemAsync(cartItemId);

        // Redirect to the Index action with the CartId
        return RedirectToAction("Index", new { cartId = cartItem.CartId });
    }

    public async Task<IActionResult> OrderConfirmation(int orderId)
    {
        // Fetch the order
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return NotFound($"Order with ID {orderId} not found.");
        }

        return View(order);
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not logged in.");
        }

        // Retrieve cartId from claims
        var cartIdClaim = User.FindFirst("cartId")?.Value;
        int? cartId = string.IsNullOrEmpty(cartIdClaim) ? null : int.Parse(cartIdClaim);

        ShoppingCartDto? cart = null;

        if (cartId.HasValue)
        {
            // Attempt to retrieve the shopping cart by cartId
            cart = await _shoppingCartRepository.GetShoppingCartByIdAsync(cartId.Value);

            if (cart == null)
            {
                // If cartId in claims is invalid, remove it
                await RemoveCartIdClaimAsync();
            }
        }

        if (cart == null)
        {
            // If cartDto is still null, fetch cart by customerId
            var customerId = await _shoppingCartRepository.GetCustomerIdByUserIdAsync(userId);
            if (!customerId.HasValue)
            {
                return BadRequest("Could not find customer for the logged-in user.");
            }

            cart = await _shoppingCartRepository.GetShoppingCartByIdAsync(cartId.Value);

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index", "ShoppingCart", new { message = "Your cart is empty." });
            }
        }

        // Calculate subtotal and taxes
        var subtotal = cart.CartItems.Sum(ci => ci.Total);

        // Fetch customer details
        var customer = await _customerRepository.GetCustomerByIdAsync(cart.CustomerId);
        if (customer == null)
        {
            return BadRequest("Customer details not found.");
        }

        var taxes = await _soapClient.CalculateTaxesAsync(subtotal, customer.Province);

        // Pass totals to ViewBag
        ViewBag.Subtotal = subtotal;
        ViewBag.Taxes = taxes;
        ViewBag.Total = subtotal + taxes;

        // Pass cart items to the view
        return View(cart.CartItems);
    }

    [HttpPost]
    public async Task<IActionResult> CheckoutItems()
    {
        // Step 1: Check if cartId exists in claims
        var cartIdClaim = User.FindFirst("cartId")?.Value;
        int? cartId = string.IsNullOrEmpty(cartIdClaim) ? null : int.Parse(cartIdClaim);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User is not logged in.");
        }

        var customerId = await _shoppingCartRepository.GetCustomerIdByUserIdAsync(userId);
        if (!customerId.HasValue)
        {
            return BadRequest("Could not find customer for the logged-in user.");
        }

        var cart = await _shoppingCartRepository.GetShoppingCartByIdAsync(cartId.Value);
        if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
        {
            return BadRequest("Your cart is empty.");
        }

        var customer = await _customerRepository.GetCustomerByIdAsync(customerId.Value);
        if (customer == null || string.IsNullOrEmpty(customer.CreditCard))
        {
            return BadRequest("Invalid customer or credit card information.");
        }

        var subtotal = cart.CartItems.Sum(ci => ci.Total);
        var taxes = await _soapClient.CalculateTaxesAsync(subtotal, customer.Province);

        var orderItems = cart.CartItems.Select(cartItem => new OrderItemDto
        {
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity,
            Price = cartItem.Price,
        }).ToList();

        var orderDto = new OrderDto
        {
            CustomerId = customerId.Value,
            DateCreated = DateTime.Today.Date,
            DateFulfilled = DateTime.Today.Date,
            Total = subtotal + taxes,
            Taxes = taxes,
            OrderItems = orderItems // Pass all order items here
        };

        var createdOrder = await _orderRepository.CreateOrderWithItemsAsync(orderDto);

        if (createdOrder == null)
        {
            return BadRequest("Failed to create the order.");
        }

        // Clear the shopping cart
        foreach (var cartItem in cart.CartItems)
        {
            await _cartItemRepository.DeleteCartItemAsync(cartItem.CartItemId);
        }

        await _shoppingCartRepository.DeleteShoppingCartAsync(cart.CartId);

        TempData["SuccessMessage"] = "Your order has been placed successfully and is being processed!";
        return RedirectToAction("OrderConfirmation", new { orderId = createdOrder.OrderId });
    }

    private async Task UpdateCartIdClaimAsync(int cartId)
    {
        var identity = User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            // Remove the existing cartId claim
            var existingCartIdClaim = identity.FindFirst("cartId");
            if (existingCartIdClaim != null)
            {
                identity.RemoveClaim(existingCartIdClaim);
            }

            // Add the new cartId claim
            identity.AddClaim(new Claim("cartId", cartId.ToString()));

            // Re-issue the authentication cookie
            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                new ClaimsPrincipal(identity));
        }
    }

    private async Task RemoveCartIdClaimAsync()
    {
        var identity = User.Identity as ClaimsIdentity;
        if (identity != null)
        {
            var existingCartIdClaim = identity.FindFirst("cartId");
            if (existingCartIdClaim != null)
            {
                identity.RemoveClaim(existingCartIdClaim);

                // Re-issue the authentication cookie
                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(identity));
            }
        }
    }
}