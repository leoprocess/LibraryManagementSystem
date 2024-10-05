using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Librarian,Admin")]
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BooksController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return View(books);
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryRepository.GetAllCategoriesAsync();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book model, IFormFile CoverImage)
        {
            if (ModelState.IsValid)
            {
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    var fileName = Path.GetFileName(CoverImage.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // 確保目錄存在
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    var filePath = Path.Combine(imagePath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverImage.CopyToAsync(stream);
                    }

                    model.CoverImagePath = "/images/" + fileName;
                }

                await _bookRepository.AddBookAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(model);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(book);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book model, IFormFile CoverImage)
        {
            if (id != model.BookId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    var fileName = Path.GetFileName(CoverImage.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // 確保目錄存在
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    // 組合完整的路徑
                    var filePath = Path.Combine(imagePath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverImage.CopyToAsync(stream);
                    }

                    model.CoverImagePath = "/images/" + fileName;
                }

                await _bookRepository.UpdateBookAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryRepository.GetAllCategoriesAsync();
            return View(model);
        }

        
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookRepository.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
