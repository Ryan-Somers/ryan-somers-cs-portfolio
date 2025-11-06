using rsH60Customer.Models;

namespace rsH60Customer.DTO;

public class OrderConfirmationViewModel
{
    public OrderDto Order { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
}