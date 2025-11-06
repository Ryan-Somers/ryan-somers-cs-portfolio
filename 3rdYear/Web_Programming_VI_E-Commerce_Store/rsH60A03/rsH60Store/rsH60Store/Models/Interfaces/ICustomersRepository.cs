namespace rsH60Store.Models.Interfaces;

public interface ICustomersRepository
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(int id);
    Task AddCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(int id);
}