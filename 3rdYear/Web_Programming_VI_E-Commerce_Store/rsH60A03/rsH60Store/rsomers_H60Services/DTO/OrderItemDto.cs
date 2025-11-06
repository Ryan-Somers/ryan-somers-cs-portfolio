namespace rsomers_H60Services.DTO;

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty; // Optional, include if you need the name
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}