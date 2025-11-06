using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using rsomers_H60Services.Models;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Models.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ServicesDBContext _context;

        public ProductCategoryRepository(ServicesDBContext context)
        {
            _context = context;
        }

        public async Task<ProductCategory> GetCategoryByIdAsync(int id)
        {
            return await _context.ProductCategories.FindAsync(id);
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesAsync()
        {
            return await _context.ProductCategories
                .Include(c => c.Products)  // Eager load the Products collection
                .OrderBy(c => c.ProdCat)
                .ToListAsync();
        }
        

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int prodCatId)
        {
            return await _context.Products
                .Where(p => p.ProdCatId == prodCatId) 
                .OrderBy(p => p.Description)          
                .ToListAsync();
        }



        public async Task AddCategoryAsync(ProductCategory category)
        {
            _context.ProductCategories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategory category)
        {
            _context.ProductCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.ProductCategories
                .Include(c => c.Products)  
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                throw new ArgumentException("Category not found.");
            }

            // Check if the category has any products
            if (category.Products.Any())
            {
                throw new InvalidOperationException("Cannot delete category because it contains products.");
            }

            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();
        }

    }
}