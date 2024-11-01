using BMS.Domain.Models.Book;

namespace BMS.Domain.Interfaces
{
    public interface IBookRepository
    {
      
        bool IsBookExists(string title, int publishedYear , string  author,string genre,int? id);

        bool AddBook(BookInfo book);
        bool EditBook(BookInfo book);
        bool DeleteBook(int bookId);

        Task<BookInfo> GetBookAsync(int bookId);
      

        void Save();
        Task SaveAsync();
    }
}
