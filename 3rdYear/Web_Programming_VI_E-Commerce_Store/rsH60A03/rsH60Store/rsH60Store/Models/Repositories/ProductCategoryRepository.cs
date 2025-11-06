using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rsH60Store.Models;
using rsH60Store.Models.Interfaces;

namespace rsH60Store.Models.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly HttpClient _client;

        public ProductCategoryRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<ProductCategory> GetCategoryByIdAsync(int id)
        {
            var response = await _client.GetAsync($"/api/productcategory/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadFromJsonAsync<ProductCategory>();
                return jsonString;
            }
            else
            {
                throw new Exception($"Failed to retrieve category: {response.ReasonPhrase}");
            }
        }


        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            var response = await _client.GetAsync("/api/ProductCategory");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategory>>();
                return jsonString;
            }
            else
            {
                throw new Exception($"Failed to retrieve categories: {response.ReasonPhrase}");
            }
        }

        public async Task AddCategoryAsync(ProductCategory category)
        {
            var jsonContent = JsonSerializer.Serialize(category);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/productcategory", content);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error responses
                throw new Exception($"Failed to add category: {response.ReasonPhrase}");
            }
        }


        public async Task UpdateCategoryAsync(ProductCategory category)
        {
            var jsonContent = JsonSerializer.Serialize(category);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/productcategory/{category.CategoryId}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update category: {response.ReasonPhrase}");
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var response = await _client.DeleteAsync($"/api/productcategory/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response
                throw new Exception($"Failed to delete category: {response.ReasonPhrase}");
            }
        }


    }
}