using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using rsH60Customer.Models;
using rsH60Customer.Models.Interfaces;

namespace rsH60Customer.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly IProductCategoryRepository _repo;

        public ProductCategoriesController(IProductCategoryRepository repo)
        {
            _repo = repo;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index()
        {
            return View(await _repo.GetAllCategoriesAsync());
        }

    }
}
