using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Librarian,Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(categories);
        }

        
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddCategoryAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(model);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            // 排除自己
            ViewBag.Categories = categories.Where(c => c.CategoryId != id);
            return View(category);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.CategoryId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateCategoryAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(model);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
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
            await _categoryRepository.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
