using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TdtuProject.Models;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Controllers
{
    [Authorize(Roles ="admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(IProductService productService, IWebHostEnvironment hostingEnvironment)
        {
            _productService = productService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _productService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if(ModelState.IsValid)
            {
                var result = await _productService.AddAsync(product);
                TempData["msg"] = result.Message;
                return RedirectToAction("Create");
            }
            var categories = await _productService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetByIdAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }
            var categories = await _productService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product product)
        {
            if(id != product.ProductId)
            {
                return NotFound();
            }
            var result = await _productService.UpdateAsync(product);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Edit));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _productService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _productService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
