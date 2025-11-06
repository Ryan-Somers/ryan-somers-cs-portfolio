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
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryRepository _repo;

        public ProductCategoryController(IProductCategoryRepository repo)
        {
            _repo = repo;
        }

        // GET: api/ProductCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategoryDTO>>> GetProductCategories()
        {
            var categories = await _repo.GetAllCategoriesAsync();

            var categoryDTOs = categories.Select(c => new ProductCategoryDTO
            {
                CategoryId = c.CategoryId,
                ProdCat = c.ProdCat,
                Products = c.Products.Select(p => new ProductInCategoryDto()
                {
                    ProductId = p.ProductId,
                    Description = p.Description
                }).ToList()
            }).ToList();

            return Ok(categoryDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategoryDTO>> GetProductCategory(int id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = new ProductCategoryDTO
            {
                CategoryId = category.CategoryId,
                ProdCat = category.ProdCat,
                Products = category.Products.Select(p => new ProductInCategoryDto()
                {
                    ProductId = p.ProductId,
                    Description = p.Description
                }).ToList()
            };

            return Ok(categoryDTO);
        }


        // PUT: api/ProductCategory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        {
            if (id != productCategory.CategoryId)
            {
                return BadRequest();
            }

            try
            {
                await _repo.UpdateCategoryAsync(productCategory); 
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductCategoryExists(id))  
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductCategory
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            await _repo.AddCategoryAsync(productCategory);  
            return CreatedAtAction(nameof(GetProductCategory), new { id = productCategory.CategoryId }, productCategory);
        }

        // DELETE: api/ProductCategory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(int id)
        {
            try
            {
                await _repo.DeleteCategoryAsync(id);  // Use repository method
            }
            catch (ArgumentException)
            {
                return NotFound();  // Category not found
            }
            catch (InvalidOperationException)
            {
                return Conflict("Cannot delete category because it contains products.");  // Conflict response
            }

            return NoContent();
        }

        // Helper method to check if a category exists
        private async Task<bool> ProductCategoryExists(int id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            return category != null;
        }
    }
}
