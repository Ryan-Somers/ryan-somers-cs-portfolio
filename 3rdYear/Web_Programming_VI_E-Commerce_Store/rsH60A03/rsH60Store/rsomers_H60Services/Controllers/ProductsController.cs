using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rsomers_H60Services.DTO;
using rsomers_H60Services.Models;
using rsomers_H60Services.Models.Interfaces;

namespace rsomers_H60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            
            var productDto = new ProductDto
            {
                ProductId = product.ProductId,
                Description = product.Description,
                Manufacturer = product.Manufacturer,
                Stock = product.Stock,
                BuyPrice = product.BuyPrice,
                SellPrice = product.SellPrice,
                ImageUrl = product.ImageUrl,
                ProdCatId = product.ProdCatId
            };

            return Ok(productDto);
        }
        
        [HttpGet("SortByBuyPrice")]
        public async Task<IActionResult> GetProductsSortedByBuyPrice([FromQuery] bool ascending = true)
        {
            var products = await _productRepository.GetAllProductsAsync();

            var sortedProducts = ascending
                ? products.OrderBy(p => p.BuyPrice).ToList()
                : products.OrderByDescending(p => p.BuyPrice).ToList();

            return Ok(sortedProducts);
        }
        
        [HttpGet("SortBySellPrice")]
        public async Task<IActionResult> GetProductsSortedBySellPrice([FromQuery] bool ascending = true)
        {
            var products = await _productRepository.GetAllProductsAsync();

            var sortedProducts = ascending
                ? products.OrderBy(p => p.SellPrice).ToList()
                : products.OrderByDescending(p => p.SellPrice).ToList();

            return Ok(sortedProducts);
        }
        
        [HttpGet("SortByStock")]
        public async Task<IActionResult> GetProductsSortedByStock([FromQuery] bool ascending = true)
        {
            var products = await _productRepository.GetAllProductsAsync();

            var sortedProducts = ascending
                ? products.OrderBy(p => p.Stock).ToList()
                : products.OrderByDescending(p => p.Stock).ToList();

            return Ok(sortedProducts);
        }
        

        [HttpGet("/api/products/search/{searchTerm:alpha}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductBySearch(string searchTerm)
        {
            var products = await _productRepository.GetProductBySearch(searchTerm);
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                ProdCatId = productDto.ProdCatId,
                Description = productDto.Description,
                Manufacturer = productDto.Manufacturer,
                Stock = productDto.Stock,
                BuyPrice = productDto.BuyPrice,
                SellPrice = productDto.SellPrice,
                ImageUrl = productDto.ImageUrl
            };

            try
            {
                await _productRepository.AddProductAsync(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDto productDto)
        {
            if (id != productDto.ProductId)
            {
                return BadRequest("Product ID mismatch.");
            }

            try
            {
                await _productRepository.UpdateProductAsync(id, productDto);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteProductAsync(id);

            return NoContent();
        }

        // GET: api/products/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        // GET: api/products/category/{categoryId}/sorted
        [HttpGet("category/{categoryId}/sorted")]
        public async Task<IActionResult> GetProductsByCategorySorted(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategorySortedAsync(categoryId);
            return Ok(products);
        }

        // PUT: api/products/{productId}/stock
        // PUT: api/products/{productId}/stock
        [HttpPut("{productId}/stock")]
        public async Task<IActionResult> UpdateProductStock(int productId, [FromBody] StockUpdateDTO stockUpdateDto)
        {
            if (stockUpdateDto == null)
            {
                return BadRequest("Invalid stock update data.");
            }

            try
            {
                await _productRepository.UpdateProductStockAsync(productId, stockUpdateDto.changeAmount);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/products/{productId}/buyprice
        [HttpPut("{productId}/buyprice")]
        public async Task<IActionResult> UpdateBuyPrice(int productId, [FromBody] decimal newBuyPrice)
        {
            try
            {
                await _productRepository.UpdateBuyPriceAsync(productId, newBuyPrice);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/products/{productId}/sellprice
        [HttpPut("{productId}/sellprice")]
        public async Task<IActionResult> UpdateSellPrice(int productId, [FromBody] decimal newSellPrice)
        {
            try
            {
                await _productRepository.UpdateSellPriceAsync(productId, newSellPrice);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //
        //     private bool ProductExists(int id)
        //     {
        //         return _context.Products.Any(e => e.ProductId == id);
        //     }
        // }
    }
}
