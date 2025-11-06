using rsomers_H60Services.Models;

namespace rsomers_H60Services.Models.Interfaces;

public interface IProductCategoryRepository
{
    Task<ProductCategory> GetCategoryByIdAsync(int id);
    Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync();
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int prodId);
    Task AddCategoryAsync(ProductCategory category);
    Task UpdateCategoryAsync(ProductCategory category);
    Task DeleteCategoryAsync(int id);
}