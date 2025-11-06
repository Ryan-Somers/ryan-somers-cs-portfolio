using Microsoft.AspNetCore.Mvc;
using rsomers_H60Services.CalculateTaxes;
using rsomers_H60Services.DTO;
using rsomers_H60Services.Models;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICheckCreditCard _checkCreditCard;
    private readonly IOrderItemRepository _orderItemRepository;

    public OrderController(IOrderRepository orderRepository, ICheckCreditCard checkCreditCard, IOrderItemRepository orderItemRepository)
    {
        _orderRepository = orderRepository;
        _checkCreditCard = checkCreditCard;
        _orderItemRepository = orderItemRepository;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet("ByDate/{date}")]
    public async Task<IActionResult> GetOrdersByDate(DateTime date)
    {
        var orders = await _orderRepository.GetByDateFulfilledAsync(date);
        var total = orders.Sum(order => order.Total);
        
        return Ok(new { orders, total });
    }

    [HttpGet("ByCustomer/{customerId}")]
    public async Task<IActionResult> GetOrdersByCustomerId(int customerId)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(customerId);
        var total = orders.Sum(order => order.Total);
        return Ok(new { orders, total });
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var order = new Order
        {
            CustomerId = orderDto.CustomerId,
            DateCreated = orderDto.DateCreated,
            DateFulfilled = orderDto.DateFulfilled,
            Total = orderDto.Total,
            Taxes = orderDto.Taxes,
        };

        await _orderRepository.AddAsync(order);

        if (orderDto.OrderItems != null)
        {
            foreach (var orderItemDto in orderDto.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = orderItemDto.ProductId,
                    Quantity = orderItemDto.Quantity,
                    Price = orderItemDto.Price
                };
                await _orderItemRepository.AddAsync(orderItem);
            }
        }

        // Return the DTO
        var result = new OrderDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            DateCreated = order.DateCreated,
            DateFulfilled = order.DateFulfilled,
            Total = order.Total,
            Taxes = order.Taxes,
            OrderItems = orderDto.OrderItems // Include items added in the request
        };

        return CreatedAtAction(nameof(GetOrderById), new { id = result.OrderId }, result);
    }





    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
    {
        if (id != order.OrderId || !ModelState.IsValid)
            return BadRequest();

        await _orderRepository.UpdateAsync(order);
        return NoContent();
    }
    
    [HttpPost("ValidateCreditCard")]
    public async Task<IActionResult> ValidateCreditCard([FromBody] string creditCardNumber)
    {
        if (string.IsNullOrWhiteSpace(creditCardNumber))
        {
            return BadRequest("Credit card number is required.");
        }

        try
        {

            var validationResult = await _checkCreditCard.ValidateCreditCardAsync(creditCardNumber);

            if (validationResult)
            {
                return Ok("Credit card is valid.");
            }
            else
            {
                return BadRequest("Credit card is invalid.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while validating the credit card: {ex.Message}");
        }
    }
    
    [HttpPost("CalculateTaxes")]
    public async Task<IActionResult> CalculateTaxes([FromBody] TaxRequestDTO taxRequest)
    {
        if (taxRequest == null || string.IsNullOrWhiteSpace(taxRequest.Province) || taxRequest.Amount <= 0)
        {
            return BadRequest("Valid province and amount are required.");
        }
    
        try
        {
            // Instantiate the SOAP client with the proper endpoint configuration
            var soapClient = new CalculateTaxes.CalculateTaxesSoapClient(
                CalculateTaxesSoapClient.EndpointConfiguration.CalculateTaxesSoap);
    
            // Call the CalculateTaxAsync method
            var taxes = await soapClient.CalculateTaxAsync(taxRequest.Amount, taxRequest.Province);
    
            return Ok(new { Taxes = taxes });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while calculating taxes: {ex.Message}");
        }
    }



}
