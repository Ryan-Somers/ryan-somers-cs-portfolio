using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rsomers_H60Services.DTO;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Models.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ServicesDBContext _context;

        public ProductRepository(ServicesDBContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProdCat) 
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<ProductDto?> GetProductDtoByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProdCatId = product.ProdCatId,
                Description = product.Description ?? string.Empty,
                Manufacturer = product.Manufacturer ?? string.Empty,
                Stock = product.Stock,
                BuyPrice = product.BuyPrice,
                SellPrice = product.SellPrice,
                ImageUrl = product.ImageUrl
            };
        }


        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProdCat)
                .OrderBy(p => p.Description)  
                .ThenBy(p => p.ProdCat.ProdCat)
                .Select(p => new ProductDto  
                {
                    ProductId = p.ProductId,
                    Description = p.Description,
                    SellPrice = p.SellPrice,
                    BuyPrice = p.BuyPrice,
                    ImageUrl = p.ImageUrl,
                    Manufacturer = p.Manufacturer,
                    Stock = p.Stock,
                    ProdCatId = p.ProdCatId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductBySearch(string searchTerm)
        {
            var product = _context.Products.OrderBy(p => p.Description).Where(p => p.Description.Contains(searchTerm)).ToListAsync();
            return await product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.ProdCatId == categoryId)
                .ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetProductsByCategorySortedAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.ProdCatId == categoryId)
                .OrderBy(p => p.Description)
                .ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            // Add the product to the database directly
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateProductAsync(int productId, ProductDto productDto)
        {
            var existingProduct = await GetProductByIdAsync(productId);
            if (existingProduct == null)
            {
                throw new ArgumentException("Product not found.");
            }

            // Update fields
            existingProduct.Description = productDto.Description;
            existingProduct.Manufacturer = productDto.Manufacturer;
            existingProduct.Stock = productDto.Stock;
            existingProduct.BuyPrice = productDto.BuyPrice;
            existingProduct.SellPrice = productDto.SellPrice;
            existingProduct.ImageUrl = productDto.ImageUrl;
            existingProduct.ProdCatId = productDto.ProdCatId;
            
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateProductStockAsync(int productId, int changeAmount)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            if (product.Stock + changeAmount < 0)
            {
                throw new Exception("Insufficient stock");
            }

            product.Stock += changeAmount;
            await _context.SaveChangesAsync();
        }


        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task UpdateBuyPriceAsync(int productId, decimal newBuyPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }
            

            // Check if the new buy price is greater than the current sell price
            if (product.SellPrice < newBuyPrice)
            {
                throw new ArgumentException("Buy price cannot be greater than the current sell price.");
            }

            // Update the buy price
            product.BuyPrice = newBuyPrice;

            await _context.SaveChangesAsync();
        }

        
        public async Task UpdateSellPriceAsync(int productId, decimal newSellPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            // Validation for sell price
            if (newSellPrice < product.BuyPrice)
            {
                throw new ArgumentException("Sell price cannot be less than the current buy price.");
            }

            // Update the sell price
            product.SellPrice = newSellPrice;

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        



    }
}
