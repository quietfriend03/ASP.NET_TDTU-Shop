using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TdtuProject.Models;
using TdtuProject.Repository.Interface;

namespace TdtuProject.Controllers
{
    [Authorize(Roles ="admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        { 
            var categories = await _categoryService.GetAllAsync(); 
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View(category);
            }
            var result = await _categoryService.AddAsync(category);
            TempData["msg"] = result.Message;
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CategoryId,CategoryName")] Category category, int id)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            var result = await _categoryService.UpdateAsync(category);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Edit));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
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
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteAsync(category);
            return RedirectToAction(nameof(Index));
        }
    }
}
