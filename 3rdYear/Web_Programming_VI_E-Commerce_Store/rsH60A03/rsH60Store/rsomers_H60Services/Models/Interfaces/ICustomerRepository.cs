namespace rsomers_H60Services.Models.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(int id);
    Task AddCustomerAsync(Customer customer);
    Task UpdateCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(int id);
}