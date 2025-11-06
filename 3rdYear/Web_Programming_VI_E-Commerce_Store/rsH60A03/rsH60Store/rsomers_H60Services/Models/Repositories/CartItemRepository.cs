using Microsoft.EntityFrameworkCore;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Models.Repositories;

public class CartItemRepository: ICartItemRepository
{
    private readonly ServicesDBContext _context;
    
    public CartItemRepository(ServicesDBContext context)
    {
        _context = context;
    }
    
    public async Task<CartItem?> GetByIdAsync(int id)
    {
        return await _context.CartItems
            .Include(ci => ci.Product)
            .Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.CartItemId == id);
    }

    public async Task<IEnumerable<CartItem>> GetByCartIdAsync(int cartId)
    {
        return await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .Include(ci => ci.Product)
            .ToListAsync();
    }

    public async Task AddAsync(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}