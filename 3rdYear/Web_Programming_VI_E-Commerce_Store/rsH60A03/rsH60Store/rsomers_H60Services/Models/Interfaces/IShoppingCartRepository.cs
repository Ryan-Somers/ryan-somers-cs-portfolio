namespace rsomers_H60Services.Models.Interfaces;

public interface IShoppingCartRepository
{
    Task<IEnumerable<ShoppingCart>> GetAllAsync();
    Task<ShoppingCart?> GetByIdAsync(int id);
    Task AddAsync(ShoppingCart cart);
    Task UpdateAsync(ShoppingCart cart); 
    Task<bool> DeleteAsync(int cartId);
    Task<Customer> GetCustomerByUserId(string userId);
}