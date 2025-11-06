using rsH60Customer.DTO;
using rsH60Customer.Models.Interfaces;

namespace rsH60Customer.Models.Repositories;

public class CartItemRepositoryRepository: ICartItemRepository
{
    private readonly HttpClient _httpClient;
    
    public CartItemRepositoryRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CartItemDto?> GetCartItemByIdAsync(int cartItemId)
    {
        var response = await _httpClient.GetAsync($"/api/CartItem/{cartItemId}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<CartItemDto>();
    }

    // Add a cart item
    public async Task<CartItemDto?> AddCartItemAsync(CartItemDto cartItemDto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/CartItem", cartItemDto);
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<CartItemDto>();
    }

    // Update a cart item
    public async Task UpdateCartItemAsync(int cartItemId, CartItemDto cartItemDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/CartItem/{cartItemId}", cartItemDto);
        response.EnsureSuccessStatusCode();
    }

    // Delete a cart item
    public async Task DeleteCartItemAsync(int cartItemId)
    {
        var response = await _httpClient.DeleteAsync($"/api/CartItem/{cartItemId}");
        response.EnsureSuccessStatusCode();
    }
}