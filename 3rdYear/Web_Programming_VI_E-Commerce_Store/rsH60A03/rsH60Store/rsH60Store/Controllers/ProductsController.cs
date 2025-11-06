using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using rsH60Store.Models;
using rsH60Store.Models.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using rsH60Store.Models.Interfaces;

namespace rsH60Store.Controllers
{
    [Authorize(Roles = "Manager,Clerk")]
    public class ProductsController : Controller
    {
        private readonly ProductValidationService _validationService;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _categoryRepository;

        public ProductsController(ProductValidationService validationService, IProductRepository productRepository,
            IProductCategoryRepository categoryRepository)
        {
            _validationService = validationService;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: Products
        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, string? searchTerm)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "ProdCat");

            var products = await _productRepository.GetAllProductsAsync();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = await _productRepository.GetProductsByCategorySortedAsync(categoryId.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = await _productRepository.GetProductBySearch(searchTerm);
            }

            return View(products);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "No categories available.");
                ViewBag.ProductCategories = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            else
            {
                ViewBag.ProductCategories = new SelectList(categories, "CategoryId", "ProdCat");
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (string.IsNullOrEmpty(product.ImageUrl))
            {
                product.ImageUrl = "/images/nike.jpg";
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(product.ProdCatId);

            if (category == null)
            {
                ModelState.AddModelError("ProdCatId", "Selected product category is not valid.");
            }
            else
            {
                product.ProdCat = category;
                ModelState.Remove("ProdCat");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.AddProductAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred while creating the product: {ex.Message}");
                }
            }

            // Ensure ViewBag is set again for re-rendering the view
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.ProductCategories = new SelectList(categories, "CategoryId", "ProdCat", product.ProdCatId);
            return View(product);
        }


        // GET: Products/EditStock/5
        public async Task<IActionResult> UpdateStock(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStock(int productId, int changeAmount)
        {
            try
            {
                await _productRepository.UpdateProductStockAsync(productId, changeAmount);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (ex.Message == "Insufficient stock")
                {
                    ModelState.AddModelError("Stock", "Stock can't be less than 0");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while updating the stock");
                }
            }

            var product = await _productRepository.GetProductByIdAsync(productId);
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
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
            await _productRepository.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBuyPrice(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBuyPrice(int productId, decimal newBuyPrice)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            try
            {
                // Update the buy price
                await _productRepository.UpdateBuyPriceAsync(productId, newBuyPrice);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex) // Catch all other exceptions
            {
                TempData["ErrorMessage"] = "Buy Price cannot be higher than the sell price.";
            }

            // Update model to reflect the new price
            product.BuyPrice = newBuyPrice;
            return View(product);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateSellPrice(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSellPrice(int productId, decimal newSellPrice)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            try
            {
                // Attempt to update the sell price
                await _productRepository.UpdateSellPriceAsync(productId, newSellPrice);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sell Price cannot be lower than buy price.";
            }

            product.SellPrice = newSellPrice;
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.ProductCategories = new SelectList(categories, "CategoryId", "ProdCat");

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(product.ProdCatId);

            if (category == null)
            {
                ModelState.AddModelError("ProdCatId", "Selected product category is not valid.");
            }
            else
            {
                product.ProdCat = category;
                ModelState.Remove("ProdCat");
            }

            try
            {
                _validationService.ValidatePrices(product.BuyPrice, product.SellPrice);


                if (ModelState.IsValid)
                {
                    await _productRepository.UpdateProductAsync(product);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("BuyPrice", ex.Message);
                ModelState.AddModelError("SellPrice", ex.Message);
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.ProductCategories = new SelectList(categories, "CategoryId", "ProdCat");
            return View(product);
        }
    }
}