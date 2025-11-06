using Microsoft.EntityFrameworkCore;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Models.Repositories;

public class CustomerRepository: ICustomerRepository
{
    public readonly ServicesDBContext _context;
    
    public CustomerRepository(ServicesDBContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return false;
        }

        // Prevent deletion if customer has a shopping cart or order
        bool hasCart = await _context.ShoppingCarts.AnyAsync(c => c.CustomerId == id);
        bool hasOrder = await _context.Orders.AnyAsync(o => o.CustomerId == id);

        if (hasCart || hasOrder)
        {
            throw new Exception("Customer cannot be deleted because they have a shopping cart or order.");
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
}