using rsH60Customer.DTO;

namespace rsH60Customer.Models.Interfaces;

public interface ICartItemRepository
{
    Task<CartItemDto?> GetCartItemByIdAsync(int cartItemId);
    Task<CartItemDto?> AddCartItemAsync(CartItemDto cartItemDto);
    Task UpdateCartItemAsync(int cartItemId, CartItemDto cartItemDto);
    Task DeleteCartItemAsync(int cartItemId);
}