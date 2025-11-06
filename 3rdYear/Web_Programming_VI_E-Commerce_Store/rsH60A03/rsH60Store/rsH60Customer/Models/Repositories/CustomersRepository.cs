using System.Text;
using System.Text.Json;
using rsH60Customer.Models.Interfaces;

namespace rsH60Customer.Models.Repositories;

public class CustomersRepository: ICustomersRepository
{
    private readonly HttpClient _client;
    
    public CustomersRepository(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        var response = await _client.GetAsync($"/api/customer");
        
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
            return jsonString;
        }
        else
        {
            throw new Exception($"Failed to retrieve customers: {response.ReasonPhrase}");
        }
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        var response = await _client.GetAsync($"/api/customer/{id}");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadFromJsonAsync<Customer>();
            return jsonString;
        }
        else
        {
            throw new Exception($"Failed to retrieve customer: {response.ReasonPhrase}");
        }
    }

    public async Task AddCustomerAsync(Customer customer)
    {
            var jsonContent = JsonSerializer.Serialize(customer);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/customer", content);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error responses
                throw new Exception($"Failed to add customer: {response.ReasonPhrase}");
            }
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        var jsonContent = JsonSerializer.Serialize(customer);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync($"/api/customer/{customer.CustomerId}", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to update customer: {response.ReasonPhrase}");
        }
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var response = await _client.DeleteAsync($"/api/customer/{id}");

        if (!response.IsSuccessStatusCode)
        {
            // Handle the error response
            throw new Exception($"Failed to delete customer: {response.ReasonPhrase}");
        }
        return true;
    }
}