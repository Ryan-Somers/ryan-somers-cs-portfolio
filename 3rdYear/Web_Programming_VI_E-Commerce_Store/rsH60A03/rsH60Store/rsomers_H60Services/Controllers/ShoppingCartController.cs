using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rsomers_H60Services.DTO;
using rsomers_H60Services.Models;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly ICustomerRepository _customerRepository;
    
    
    public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, ICustomerRepository customerRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _orderItemRepository = orderItemRepository;
        _customerRepository = customerRepository;
        
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShoppingCartById(int id)
    {
        var cart = await _shoppingCartRepository.GetByIdAsync(id);
        if (cart == null)
            return NotFound();

        // Map to DTO
        var cartDto = new ShoppingCartDto
        {
            CartId = cart.CartId,
            CustomerId = cart.CustomerId,
            DateCreated = cart.DateCreated,
            CartItems = cart.CartItems.Select(ci => new CartItemDto
            {
                CartItemId = ci.CartItemId,
                CartId = ci.CartId,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Description ?? "Unknown Product", // Include ProductName
                Quantity = ci.Quantity,
                Price = ci.Price,
                Total = ci.Quantity * ci.Price
            }).ToList()
        };

        return Ok(cartDto);
    }


    [HttpPost]
    public async Task<IActionResult> AddShoppingCart([FromBody] ShoppingCartDto cartDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (cartDto.CustomerId <= 0)
            return BadRequest("CustomerId is required.");

        // Create a new ShoppingCart entity
        var cart = new ShoppingCart
        {
            CustomerId = cartDto.CustomerId,
            DateCreated = cartDto.DateCreated
        };

        await _shoppingCartRepository.AddAsync(cart);

        // Return the created cart
        cartDto.CartId = cart.CartId;
        return CreatedAtAction(nameof(GetShoppingCartById), new { id = cartDto.CartId }, cartDto);
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShoppingCart(int id, [FromBody] ShoppingCart cart)
    {
        if (id != cart.CartId || !ModelState.IsValid)
            return BadRequest();

        var existingCart = await _shoppingCartRepository.GetByIdAsync(id);
        if (existingCart == null)
            return NotFound();

        await _shoppingCartRepository.UpdateAsync(cart);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShoppingCart(int id)
    {
        var cart = await _shoppingCartRepository.GetByIdAsync(id);
        if (cart == null)
            return NotFound();

        if (cart.CartItems.Any())
            return BadRequest("Cannot delete a cart with associated cart items.");

        await _shoppingCartRepository.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetCustomerByUserId(string userId)
    {
        var customer = await _shoppingCartRepository.GetCustomerByUserId(userId);
        if (customer == null)
        {
            return NotFound("Customer not found.");
        }

        return Ok(new { customer.CustomerId, customer.FirstName, customer.LastName });
    }
}