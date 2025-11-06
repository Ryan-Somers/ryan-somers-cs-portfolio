using rsH60Customer.DTO;

namespace rsH60Customer.Models.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCartDto?> GetShoppingCartByIdAsync(int cartId);
    Task<ShoppingCartDto?> AddShoppingCartAsync(ShoppingCartDto cartDto);
    Task UpdateShoppingCartAsync(int cartId, ShoppingCartDto cartDto);
    Task DeleteShoppingCartAsync(int cartId);
    Task<int?> GetCustomerIdByUserIdAsync(string userId);

    Task<bool> CartExistsAsync(int cartId);
    Task<ShoppingCartDto> GetOrCreateCartAsync(string userId);

}