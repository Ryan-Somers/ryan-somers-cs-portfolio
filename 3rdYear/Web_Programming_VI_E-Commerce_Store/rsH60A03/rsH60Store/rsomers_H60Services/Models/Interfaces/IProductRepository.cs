using rsomers_H60Services.DTO;
using rsomers_H60Services.Models;

namespace rsomers_H60Services.Models.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int id);
    Task<ProductDto?> GetProductDtoByIdAsync(int id);

    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    
    Task<IEnumerable<Product>> GetProductBySearch(string searchTerm);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetProductsByCategorySortedAsync(int categoryId);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(int productId, ProductDto product);
    Task DeleteProductAsync(int id);
    Task UpdateProductStockAsync(int productId, int changeAmount);

    Task UpdateBuyPriceAsync(int productId, decimal newBuyPrice);
    Task UpdateSellPriceAsync(int productId, decimal newSellPrice);


}