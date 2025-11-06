namespace rsomers_H60Services.Models.Interfaces;

public interface IOrderItemRepository
{
    Task<OrderItem?> GetByIdAsync(int orderItemId);
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);
    Task<OrderItem> AddAsync(OrderItem orderItem);
    Task UpdateAsync(OrderItem orderItem);
}