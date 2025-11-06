using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using rsH60Store.Models;
using rsH60Store.Models.Interfaces;
using rsH60Store.Models.Repositories;

namespace rsH60Store.Controllers
{
    [Authorize(Roles = "Manager,Clerk")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryRepository _categoryRepository;

        public ProductCategoryController(IProductCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(categories);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind("ProdCat")] ProductCategory category)
        {
            
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            
            return View(category);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _categoryRepository.GetCategoryByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _categoryRepository.DeleteCategoryAsync(id);
                return RedirectToAction(nameof(Index)); 
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;  
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "There are products in this category. Please delete the products first.";
            }
            
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View("Delete", category);  
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            try
            {
                await _categoryRepository.UpdateCategoryAsync(category);
                return RedirectToAction(nameof(Index)); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the category: " + ex.Message);
                return View(category);
            }
        }
    }
}