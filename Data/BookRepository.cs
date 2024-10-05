using Dapper;
using LibraryManagementSystem.Models;
using System.Data;

namespace LibraryManagementSystem.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection _db;

        public BookRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            string sql = @"SELECT b.*, c.CategoryName 
                   FROM Books b
                   INNER JOIN Categories c ON b.CategoryId = c.CategoryId";
            return await _db.QueryAsync<Book>(sql);
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            string sql = @"SELECT b.*, c.CategoryName 
                   FROM Books b
                   INNER JOIN Categories c ON b.CategoryId = c.CategoryId
                   WHERE b.BookId = @BookId";
            return await _db.QueryFirstOrDefaultAsync<Book>(sql, new { BookId = bookId });
        }

        public async Task AddBookAsync(Book book)
        {
            string sql = @"INSERT INTO Books (Title, Author, ISBN, CategoryId, PublicationDate, Description, CoverImagePath)
                       VALUES (@Title, @Author, @ISBN, @CategoryId, @PublicationDate, @Description, @CoverImagePath)";
            await _db.ExecuteAsync(sql, book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            string sql = @"UPDATE Books SET Title=@Title, Author=@Author, ISBN=@ISBN, 
                       CategoryId=@CategoryId, PublicationDate=@PublicationDate, 
                       Description=@Description, CoverImagePath=@CoverImagePath
                       WHERE BookId=@BookId";
            await _db.ExecuteAsync(sql, book);
        }

        public async Task DeleteBookAsync(int bookId)
        {
            string sql = "DELETE FROM Books WHERE BookId = @BookId";
            await _db.ExecuteAsync(sql, new { BookId = bookId });
        }
    }
}
