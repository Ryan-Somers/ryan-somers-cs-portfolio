using Microsoft.EntityFrameworkCore;
using rsomers_H60Services.DTO;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Models.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly ServicesDBContext _context;

    public OrderRepository(ServicesDBContext context)
    {
        _context = context;
    }
    
    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems) // Include related OrderItems
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(int customerId)
    {
        var orders = await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();

        var orderDtos = orders.Select(order => new OrderDto
        {
            OrderId = order.OrderId,
            CustomerId = order.CustomerId,
            DateCreated = order.DateCreated,
            DateFulfilled = order.DateFulfilled,
            Total = order.Total,
            Taxes = order.Taxes,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                OrderItemId = oi.OrderItemId,
                OrderId = oi.OrderId,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Description, // Map ProductName
                Quantity = oi.Quantity,
                Price = oi.Price
            }).ToList()
        }).ToList();

        return orderDtos; // Return transformed DTOs
    }



    public async Task<IEnumerable<Order>> GetByDateFulfilledAsync(DateTime dateCreated)
    {
        return await _context.Orders
            .Where(o => o.DateFulfilled.HasValue && o.DateFulfilled.Value.Date == dateCreated.Date)
            .Include(o => o.OrderItems) // Include related OrderItems
            .ToListAsync();
    }

    public async Task<Order> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}