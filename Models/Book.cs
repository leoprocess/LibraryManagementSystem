namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int? BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int CategoryId { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Description { get; set; }
        public string? CoverImagePath { get; set; }
        public string? CategoryName { get; set; }

    }
}
