namespace rsH60Customer.Models.Interfaces;

public interface IProductCategoryRepository
{
    Task<ProductCategory> GetCategoryByIdAsync(int id);
    Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync();
    Task AddCategoryAsync(ProductCategory category);
    Task UpdateCategoryAsync(ProductCategory category);
    Task DeleteCategoryAsync(int id);
}