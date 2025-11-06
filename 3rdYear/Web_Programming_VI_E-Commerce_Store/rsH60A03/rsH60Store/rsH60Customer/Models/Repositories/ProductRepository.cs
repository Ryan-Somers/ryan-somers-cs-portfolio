using System.Text;
using System.Text.Json;
using rsH60Customer.Models.Interfaces;


namespace rsH60Customer.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly HttpClient _client;

        public ProductRepository( HttpClient client)
        {
            _client = client;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/products/{id}"); // API endpoint
            
            if (response.IsSuccessStatusCode)
            {
                var productList = await response.Content.ReadFromJsonAsync<Product>();
                return productList;
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch products. Status code: {response.StatusCode}");
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/products"); // API endpoint
            
            if (response.IsSuccessStatusCode)
            {
                var productList = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
                return productList;
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch products. Status code: {response.StatusCode}");
            }
        }

        public async Task<IEnumerable<Product>> GetProductBySearch(string searchTerm)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/products/search/{searchTerm}"); // API endpoint
            
            if (response.IsSuccessStatusCode)
            {
                var productList = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
                return productList;
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch products. Status code: {response.StatusCode}");
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByCategorySortedAsync(int categoryId)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/products/category/{categoryId}/sorted"); // API endpoint
            
            if (response.IsSuccessStatusCode)
            {
                var productList = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
                return productList;
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch products. Status code: {response.StatusCode}");
            }
        }

        public async Task AddProductAsync(Product product) {
        // Serialize the ProductDto object to JSON
        var jsonContent = JsonSerializer.Serialize(product);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Send the POST request to the API
        var response = await _client.PostAsync("/api/Products/", content);

        if (!response.IsSuccessStatusCode) {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to add product: {response.StatusCode} - {response.ReasonPhrase} | Error Details: {errorContent}"); 
        } 
        
        }


        public async Task UpdateProductAsync(Product product)
        {
            var jsonContent = JsonSerializer.Serialize(product);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _client.PutAsync($"/api/products/{product.ProductId}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update product: {response.ReasonPhrase}");
            }
        }
        
        public async Task UpdateProductStockAsync(int productId, int changeAmount)
        {
            var stockUpdate = new { ChangeAmount = changeAmount };
            
            var jsonContent = JsonSerializer.Serialize(stockUpdate);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _client.PutAsync($"/api/products/{productId}/stock", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update stock: {response.ReasonPhrase}");
            }
        }
        
        public async Task DeleteProductAsync(int productId)
        {
            var response = await _client.DeleteAsync($"/api/products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"There are products in this category. Please delete them first.");
            }
        }

        
        public async Task UpdateBuyPriceAsync(int productId, decimal newBuyPrice)
        {
            // Create a payload with the new buy price
            var jsonContent = JsonSerializer.Serialize(newBuyPrice);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/products/{productId}/buyprice", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update buy price: {response.ReasonPhrase}");
            }
        }
        
        public async Task UpdateSellPriceAsync(int productId, decimal newSellPrice)
        {
            // Create a payload with the new sell price
            var jsonContent = JsonSerializer.Serialize(newSellPrice);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/products/{productId}/sellprice", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update sell price: {response.ReasonPhrase}");
            }
        }

    }
}
