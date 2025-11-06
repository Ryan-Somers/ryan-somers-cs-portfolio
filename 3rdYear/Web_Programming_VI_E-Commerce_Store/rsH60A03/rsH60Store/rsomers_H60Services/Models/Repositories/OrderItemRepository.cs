using Microsoft.EntityFrameworkCore;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Models.Repositories;

public class OrderItemRepository: IOrderItemRepository
{
    private readonly ServicesDBContext _context;

    public OrderItemRepository(ServicesDBContext context)
    {
        _context = context;
    }

    public async Task<OrderItem?> GetByIdAsync(int orderItemId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Product) // Include related Product if needed
            .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
    {
        return await _context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .Include(oi => oi.Product) // Include related Product if needed
            .ToListAsync();
    }

    public async Task<OrderItem> AddAsync(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();
        return orderItem;
    }

    public async Task UpdateAsync(OrderItem orderItem)
    {
        _context.OrderItems.Update(orderItem);
        await _context.SaveChangesAsync();
    }
}