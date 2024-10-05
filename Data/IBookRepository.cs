using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int bookId);
    }
}
