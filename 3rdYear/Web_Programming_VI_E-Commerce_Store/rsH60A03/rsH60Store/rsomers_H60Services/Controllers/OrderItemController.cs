using Microsoft.AspNetCore.Mvc;
using rsomers_H60Services.DTO;
using rsomers_H60Services.Models;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderItemController : ControllerBase
{
    private readonly IOrderItemRepository _orderItemRepository;

    public OrderItemController(IOrderItemRepository orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderItemById(int id)
    {
        var orderItem = await _orderItemRepository.GetByIdAsync(id);
        if (orderItem == null)
            return NotFound();

        return Ok(orderItem);
    }
    
    [HttpGet("ByOrder/{orderId}")]
    public async Task<IActionResult> GetOrderItemsByOrderId(int orderId)
    {
        var orderItems = await _orderItemRepository.GetByOrderIdAsync(orderId);

        if (orderItems == null || !orderItems.Any())
        {
            return NotFound($"No order items found for OrderId {orderId}");
        }

        var orderItemDtos = orderItems.Select(oi => new OrderItemDto
        {
            OrderItemId = oi.OrderItemId,
            OrderId = oi.OrderId,
            ProductId = oi.ProductId,
            ProductName = oi.Product.Description, // Include ProductName if needed
            Quantity = oi.Quantity,
            Price = oi.Price
        });

        return Ok(orderItemDtos);
    }



    [HttpPost]
    public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemDto orderItemDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Map DTO to entity
        var orderItem = new OrderItem
        {
            OrderId = orderItemDto.OrderId,
            ProductId = orderItemDto.ProductId,
            Quantity = orderItemDto.Quantity,
            Price = orderItemDto.Price
        };

        // Save the entity
        await _orderItemRepository.AddAsync(orderItem);

        // Map the saved entity back to a DTO
        var savedOrderItemDto = new OrderItemDto
        {
            OrderItemId = orderItem.OrderItemId,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            Price = orderItem.Price
        };

        return CreatedAtAction(nameof(GetOrderItemById), new { id = savedOrderItemDto.OrderItemId }, savedOrderItemDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItem orderItem)
    {
        if (id != orderItem.OrderItemId || !ModelState.IsValid)
            return BadRequest();

        await _orderItemRepository.UpdateAsync(orderItem);
        return NoContent();
    }
}
