using rsH60Customer.DTO;
using rsH60Customer.Models.Interfaces;

namespace rsH60Customer.Models.Repositories;

public class OrderItemRepository: IOrderItemRepository
{
    private readonly HttpClient _httpClient;

    public OrderItemRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderItem?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/OrderItem/{id}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<OrderItem>();
    }
    
    public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        var response = await _httpClient.GetAsync($"/api/OrderItem/ByOrder/{orderId}");

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error fetching order items: {response.ReasonPhrase}");
            return Enumerable.Empty<OrderItemDto>();
        }

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<OrderItemDto>>();

        if (result == null)
        {
            Console.WriteLine("Deserialization returned null.");
            return Enumerable.Empty<OrderItemDto>();
        }

        return result;
    }




    public async Task<OrderItemDto?> AddOrderItemAsync(OrderItemDto orderItemDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/OrderItem", orderItemDto);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<OrderItemDto>();
    }
    
    public async Task UpdateAsync(int id, OrderItem orderItem)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/OrderItem/{id}", orderItem);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<bool> AddOrderItemsAsync(IEnumerable<OrderItemDto> orderItems)
    {
        foreach (var item in orderItems)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/OrderItem", item);
            if (!response.IsSuccessStatusCode)
            {
                return false; // Return false if any item fails
            }
        }
        return true; // Return true if all items are added successfully
    }

}