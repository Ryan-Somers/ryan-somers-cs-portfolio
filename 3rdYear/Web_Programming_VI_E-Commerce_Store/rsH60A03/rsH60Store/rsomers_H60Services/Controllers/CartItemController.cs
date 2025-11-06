using System;
using System.Collections.Generic;
using System.Linq;
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
public class CartItemController : Controller
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IProductRepository _productRepository;
    
    public CartItemController(ICartItemRepository cartItemRepository, IProductRepository productRepository)
    {
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
    }
    // GET
    [HttpGet("{id}")]
public async Task<IActionResult> GetCartItemById(int id)
{
    var cartItem = await _cartItemRepository.GetByIdAsync(id);
    if (cartItem == null)
        return NotFound();

    // Map to DTO
    var cartItemDto = new CartItemDto
    {
        CartItemId = cartItem.CartItemId,
        CartId = cartItem.CartId,
        ProductId = cartItem.ProductId,
        Quantity = cartItem.Quantity,
        Price = cartItem.Price
    };

    return Ok(cartItemDto);
}

    [HttpPost]
    public async Task<IActionResult> AddCartItem([FromBody] CartItemDto cartItemDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check stock before adding the item
        var product = await _productRepository.GetProductDtoByIdAsync(cartItemDto.ProductId);
        if (product == null || product.Stock < cartItemDto.Quantity)
            return BadRequest("Insufficient stock for the product.");

        // Deduct stock
        product.Stock -= cartItemDto.Quantity;
        await _productRepository.UpdateProductAsync(product.ProductId, product);

        // Create CartItem from DTO
        var cartItem = new CartItem
        {
            CartId = cartItemDto.CartId,
            ProductId = cartItemDto.ProductId,
            Quantity = cartItemDto.Quantity,
            Price = product.SellPrice // Use product's SellPrice
        };

        await _cartItemRepository.AddAsync(cartItem);

        // Populate calculated fields
        cartItemDto.CartItemId = cartItem.CartItemId;
        cartItemDto.Price = cartItem.Price;
        cartItemDto.Total = cartItem.Quantity * cartItem.Price;

        return CreatedAtAction(nameof(GetCartItemById), new { id = cartItem.CartItemId }, cartItemDto);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItemDto cartItemDto)
    {
        if (!ModelState.IsValid || id != cartItemDto.CartItemId)
            return BadRequest();

        var existingCartItem = await _cartItemRepository.GetByIdAsync(id);
        if (existingCartItem == null)
            return NotFound();

        // Check stock adjustment
        var product = await _productRepository.GetProductDtoByIdAsync(cartItemDto.ProductId);
        if (product == null)
            return BadRequest("Product not found.");

        var stockAdjustment = cartItemDto.Quantity - existingCartItem.Quantity;
        if (product.Stock < stockAdjustment)
            return BadRequest("Insufficient stock for the product.");

        // Adjust stock
        product.Stock -= stockAdjustment;
        await _productRepository.UpdateProductAsync(product.ProductId, product);

        // Update the CartItem
        existingCartItem.Quantity = cartItemDto.Quantity;
        existingCartItem.Price = cartItemDto.Price;

        // Calculate Total dynamically
        var total = existingCartItem.Quantity * existingCartItem.Price;

        await _cartItemRepository.UpdateAsync(existingCartItem);

        return Ok(new { Total = total }); // Optionally return the calculated total
    }





    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCartItem(int id)
    {
        var cartItem = await _cartItemRepository.GetByIdAsync(id);
        if (cartItem == null)
            return NotFound();

        // Return stock to the product
        var productDto = await _productRepository.GetProductDtoByIdAsync(cartItem.ProductId);
        if (productDto != null)
        {
            productDto.Stock += cartItem.Quantity;
            await _productRepository.UpdateProductAsync(productDto.ProductId, productDto);
        }

        // Remove the cart item
        await _cartItemRepository.DeleteAsync(id);
        return NoContent();
    }

}