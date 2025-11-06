using rsH60Customer.DTO;

namespace rsH60Customer.Models.Interfaces;

public interface IOrderRepository
{
    Task<OrderDto?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByDateFulfilledAsync(DateTime date);
    Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(int customerId);
    Task<OrderDto?> AddOrderAsync(OrderDto orderDto);
    Task UpdateAsync(int id, Order order);
    Task<OrderDto?> CreateOrderWithItemsAsync(OrderDto orderDto);

}