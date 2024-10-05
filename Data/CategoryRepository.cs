using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnection _db;

        public CategoryRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            string sql = @"
        SELECT c.*, pc.CategoryName AS ParentCategoryName
        FROM Categories c
        LEFT JOIN Categories pc ON c.ParentCategoryId = pc.CategoryId";
            return await _db.QueryAsync<Category>(sql);
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            string sql = @"SELECT * FROM Categories WHERE CategoryId = @CategoryId";
            return await _db.QueryFirstOrDefaultAsync<Category>(sql, new { CategoryId = categoryId });
        }

        public async Task AddCategoryAsync(Category category)
        {
            string sql = @"INSERT INTO Categories (CategoryName, ParentCategoryId)
                           VALUES (@CategoryName, @ParentCategoryId)";
            await _db.ExecuteAsync(sql, category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            string sql = @"UPDATE Categories SET CategoryName = @CategoryName, ParentCategoryId = @ParentCategoryId
                           WHERE CategoryId = @CategoryId";
            await _db.ExecuteAsync(sql, category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            string sql = @"DELETE FROM Categories WHERE CategoryId = @CategoryId";
            await _db.ExecuteAsync(sql, new { CategoryId = categoryId });
        }

        public async Task<IEnumerable<Category>> GetCategoryHierarchyAsync()
        {
            string sql = @"WITH CategoryCTE AS (
                                SELECT CategoryId, CategoryName, ParentCategoryId, 0 AS Level
                                FROM Categories
                                WHERE ParentCategoryId IS NULL
                                UNION ALL
                                SELECT c.CategoryId, c.CategoryName, c.ParentCategoryId, Level + 1
                                FROM Categories c
                                INNER JOIN CategoryCTE cte ON c.ParentCategoryId = cte.CategoryId
                            )
                            SELECT * FROM CategoryCTE ORDER BY Level, CategoryName";
            return await _db.QueryAsync<Category>(sql);
        }
    }
}
