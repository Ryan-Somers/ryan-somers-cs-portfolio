using rsomers_H60Services.DTO;

namespace rsomers_H60Services.Models.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int orderId);
    Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(int customerId);
    Task<IEnumerable<Order>> GetByDateFulfilledAsync(DateTime dateFulfilled);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
}