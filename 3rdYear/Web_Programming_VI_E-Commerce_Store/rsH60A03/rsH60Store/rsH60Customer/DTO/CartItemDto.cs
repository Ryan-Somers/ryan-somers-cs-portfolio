using rsH60Customer.Models;

namespace rsH60Customer.DTO;

public class CartItemDto
{
    public int CartId { get; set; }
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty; // Default empty to avoid nulls
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    private decimal _total;

    public decimal Total
    {
        get => Quantity * Price; // Automatically calculates the total when getting
        set => _total = value;   // Allows setting the total explicitly
    }

    
    
}