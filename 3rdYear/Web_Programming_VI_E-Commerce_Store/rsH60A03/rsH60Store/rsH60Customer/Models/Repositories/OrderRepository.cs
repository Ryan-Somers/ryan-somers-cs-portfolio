using rsH60Customer.DTO;
using rsH60Customer.Models.Interfaces;

namespace rsH60Customer.Models.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly HttpClient _httpClient;

    public OrderRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/Order/{id}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<OrderDto>();
    }

    public async Task<IEnumerable<Order>> GetByDateFulfilledAsync(DateTime date)
    {
        var response = await _httpClient.GetAsync($"/api/Order/ByDate/{date:yyyy-MM-dd}");
        if (!response.IsSuccessStatusCode) return Enumerable.Empty<Order>();

        return await response.Content.ReadFromJsonAsync<IEnumerable<Order>>() ?? Enumerable.Empty<Order>();
    }

    public async Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(int customerId)
    {
        var response = await _httpClient.GetAsync($"/api/Order/ByCustomer/{customerId}");
        if (!response.IsSuccessStatusCode) return Enumerable.Empty<OrderDto>();

        return await response.Content.ReadFromJsonAsync<IEnumerable<OrderDto>>() ?? Enumerable.Empty<OrderDto>();
    }

    public async Task<OrderDto?> AddOrderAsync(OrderDto orderDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Order", orderDto);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<OrderDto>();
    }
    
    public async Task<OrderDto?> CreateOrderWithItemsAsync(OrderDto orderDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Order", orderDto);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<OrderDto>();
    }



    public async Task UpdateAsync(int id, Order order)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/Order/{id}", order);
        response.EnsureSuccessStatusCode();
    }

    public Task<OrderDto?> CreateOrderWithItemsAsync(OrderDto orderDto, IEnumerable<OrderItemDto> orderItemsDto)
    {
        throw new NotImplementedException();
    }
    
    
}