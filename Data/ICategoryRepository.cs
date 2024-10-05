using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);
        Task<IEnumerable<Category>> GetCategoryHierarchyAsync();
    }
}
