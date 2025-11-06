using rsH60Customer.DTO;

namespace rsH60Customer.Models.Interfaces;

public interface IOrderItemRepository
{
    Task<OrderItem?> GetByIdAsync(int id);

    Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(int orderId);
    Task<OrderItemDto?> AddOrderItemAsync(OrderItemDto orderItemDto);
    Task UpdateAsync(int id, OrderItem orderItem);
    Task<bool> AddOrderItemsAsync(IEnumerable<OrderItemDto> orderItems);
}