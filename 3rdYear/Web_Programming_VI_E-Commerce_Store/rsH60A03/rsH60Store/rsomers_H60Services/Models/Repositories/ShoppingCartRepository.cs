using Microsoft.EntityFrameworkCore;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Models.Repositories;

public class ShoppingCartRepository: IShoppingCartRepository
{
    private readonly ServicesDBContext _context;

    public ShoppingCartRepository(ServicesDBContext context)
    {
        _context = context;
    }
    
    public Task<IEnumerable<ShoppingCart>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ShoppingCart?> GetByIdAsync(int id)
    {
        return await _context.ShoppingCarts
            .Include(c => c.CartItems) // Include related CartItems
            .ThenInclude(ci => ci.Product) // Include related Product
            .FirstOrDefaultAsync(c => c.CartId == id);
    }



    public async Task AddAsync(ShoppingCart cart)
    {
        await _context.ShoppingCarts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ShoppingCart cart)
    {
        _context.ShoppingCarts.Update(cart);
        await _context.SaveChangesAsync();
    }


    public async Task<bool> DeleteAsync(int cartId)
    {
        var cart = await _context.ShoppingCarts.FindAsync(cartId);
        if (cart == null)
        {
            return false; // Cart not found
        }

        _context.ShoppingCarts.Remove(cart);
        await _context.SaveChangesAsync();
        return true; // Cart successfully deleted
    }

    
    public async Task<Customer> GetCustomerByUserId(string userId)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with UserID {userId} not found.");
        }

        return customer;
    }
}