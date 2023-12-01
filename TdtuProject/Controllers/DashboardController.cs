using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TdtuProject.Models;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public DashboardController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Display()
        {
            IEnumerable<Category> categories = await _categoryService.GetAllAsync();
            Dictionary<Category, IEnumerable<Product>> productsByCategory = new Dictionary<Category, IEnumerable<Product>>();
            foreach(var category in categories)
            {
                IEnumerable<Product> categoryProduct = await _productService.GetByCategoryAsync(category.CategoryId);
                productsByCategory.Add(category, categoryProduct);
            }
            TempData["Categories"] = categories;
            TempData["ProductByCategory"] = productsByCategory;
            return View(categories);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Product choosedProduct = await _productService.GetByIdAsync(id);
            if(choosedProduct == null)
            {
                return NotFound();
            }
            IEnumerable<Product> relatedProduct = await _productService.GetRelatedProduct(choosedProduct.CategoryId, choosedProduct.ProductId);
            TempData["Detail"] = choosedProduct;
            TempData["Related"] = relatedProduct;
            return View();
        }

        public async Task<IActionResult> AllProduct(int? categoryId, string sortBy)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            var products = categoryId.HasValue
                ? await _productService.GetByCategoryAsync(categoryId.Value)
                : await _productService.GetAllAsync();

            switch (sortBy)
            {
                case "asc":
                    products = products.OrderBy(p => p.ProductPrice).ToList();
                    break;
                case "desc":
                    products = products.OrderByDescending(p => p.ProductPrice).ToList();
                    break;
            }

            return View(products);
        }
    }
}
