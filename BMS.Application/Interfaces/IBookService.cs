using BMS.Application.Result;
using BMS.Domain.Models.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
