using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rsH60Customer.DTO;
using rsH60Customer.Models.Interfaces;

namespace rsH60Customer.Models.Repositories;

public class ShoppingCartRepository: IShoppingCartRepository
{
    private readonly HttpClient _httpClient;

    public ShoppingCartRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Get a shopping cart by ID
    public async Task<ShoppingCartDto?> GetShoppingCartByIdAsync(int cartId)
    {
        var response = await _httpClient.GetAsync($"/api/ShoppingCart/{cartId}");
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<ShoppingCartDto>();
    }

    // Add a new shopping cart
    public async Task<ShoppingCartDto?> AddShoppingCartAsync(ShoppingCartDto cartDto)
    {
        // Prepare the payload with valid CustomerId, UserId, and empty CartItems
        var payload = new ShoppingCartDto
        {
            CustomerId = cartDto.CustomerId,
            UserId = cartDto.UserId,
            DateCreated = DateTime.UtcNow,
            CartItems = new List<CartItemDto>() // Start with an empty cart
        };

        // Send the POST request
        var response = await _httpClient.PostAsJsonAsync("/api/ShoppingCart", payload);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error creating shopping cart: {error}");
            return null;
        }

        return await response.Content.ReadFromJsonAsync<ShoppingCartDto>();
    }


    // Update an existing shopping cart
    public async Task UpdateShoppingCartAsync(int cartId, ShoppingCartDto cartDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/ShoppingCart/{cartId}", cartDto);
        response.EnsureSuccessStatusCode();
    }

    // Delete a shopping cart
    public async Task DeleteShoppingCartAsync(int cartId)
    {
        var response = await _httpClient.DeleteAsync($"/api/ShoppingCart/{cartId}");
        response.EnsureSuccessStatusCode();
    }
    
    // Helper method to get the CustomerId from UserId
    public async Task<int?> GetCustomerIdByUserIdAsync(string userId)
    {
        // Call the backend API to get the customer associated with the UserId
        var response = await _httpClient.GetAsync($"/api/ShoppingCart/GetByUserId/{userId}");
        if (!response.IsSuccessStatusCode)
        {
            return null; // Return null if the customer is not found
        }

        var customer = await response.Content.ReadFromJsonAsync<Customer>();
        return customer?.CustomerId;
    }
    
    public async Task<ShoppingCartDto> GetOrCreateCartAsync(string userId)
    {
        var customerId = await GetCustomerIdByUserIdAsync(userId);
        if (!customerId.HasValue)
        {
            throw new InvalidOperationException("Customer not found.");
        }

        var cart = await GetShoppingCartByIdAsync(customerId.Value);
        if (cart != null)
        {
            return cart;
        }

        // Create a new cart
        return await AddShoppingCartAsync(new ShoppingCartDto
        {
            CustomerId = customerId.Value,
            DateCreated = DateTime.UtcNow,
            CartItems = new List<CartItemDto>()
        });
    }
    
    public async Task<bool> CartExistsAsync(int cartId)
    {
        return await _httpClient.GetAsync($"/api/ShoppingCart/{cartId}") is { IsSuccessStatusCode: true };
    }
    
    


}