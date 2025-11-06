namespace rsomers_H60Services.DTO;

public class ShoppingCartDto
{
    public int CartId { get; set; }
    public int CustomerId { get; set; }
    public string UserId { get; set; }
    public DateTime DateCreated { get; set; }
    public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
}