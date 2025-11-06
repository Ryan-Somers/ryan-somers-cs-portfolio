namespace rsH60Store.Models.Interfaces;

public interface IProductCategoryRepository
{
    Task<ProductCategory> GetCategoryByIdAsync(int id);
    Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync();
    Task AddCategoryAsync(ProductCategory category);
    Task UpdateCategoryAsync(ProductCategory category);
    Task DeleteCategoryAsync(int id);
}