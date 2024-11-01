using BMS.Application.Result;
using BMS.Domain.Models.Book;


namespace BMS.Application.Interfaces
{
    public interface IBookService
    {
        Task<ResultSet<BookInfo>> AddBookAsync(BookInfo book);
        Task<ResultSet<BookInfo>> EditBookAsync(BookInfo book);
        Task<ResultSet> DeleteBookAsync(int bookId);
        Task<ResultSet<BookInfo>> GetBookAsync(int bookId);     

    }
}
