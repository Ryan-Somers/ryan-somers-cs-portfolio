using rsomers_H60Services.DTO;

namespace rsH60Store.Models.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<IEnumerable<Product>> GetProductBySearch(string searchTerm);
    Task<IEnumerable<Product>> GetProductsByCategorySortedAsync(int categoryId);
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(int id);
    Task UpdateProductStockAsync(int productId, int changeAmount);
    Task UpdateBuyPriceAsync(int productId, decimal newBuyPrice);
    Task UpdateSellPriceAsync(int productId, decimal newSellPrice);


}