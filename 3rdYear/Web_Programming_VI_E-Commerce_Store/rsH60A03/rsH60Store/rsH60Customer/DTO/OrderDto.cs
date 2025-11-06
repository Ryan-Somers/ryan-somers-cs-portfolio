namespace rsH60Customer.DTO;

public class OrderDto
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateFulfilled { get; set; }
    public decimal Total { get; set; }
    public decimal Taxes { get; set; }
    
    public List<OrderItemDto> OrderItems { get; set; }
}