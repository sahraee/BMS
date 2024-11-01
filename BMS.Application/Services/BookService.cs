using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Application.Interfaces;
using BMS.Application.Result;
using BMS.Domain;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.Book;

namespace BMS.Application.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this._bookRepository = bookRepository;
        }

        
        public async Task<ResultSet<BookInfo>> AddBookAsync(BookInfo book)
        {
            if (_bookRepository.IsBookExists(book.Title, book.PublishedYear, book.Author, book.Genre, null))
                return new ResultSet<BookInfo>()
                {
                    IsSucceed = false,
                    Message = Constants.Messages.DuplicateData
                };

            if (!_bookRepository.AddBook(book))
                return new ResultSet<BookInfo>() { IsSucceed = false, Message = Constants.Messages.BookNotInserted, Data = book };


            try { await _bookRepository.SaveAsync(); }

            catch (Exception e) { return new ResultSet<BookInfo>() { IsSucceed = false, Message = e.Message }; }

            return new ResultSet<BookInfo>()
            {
                IsSucceed = true,
                Message = string.Empty,
                Data = book
            };
        }

        public async Task<ResultSet<BookInfo>> EditBookAsync(BookInfo book)
        {
            if (_bookRepository.IsBookExists(book.Title, book.PublishedYear, book.Author, book.Genre, book.Id))
                return new ResultSet<BookInfo>()
                {
                    IsSucceed = false,
                    Message = Constants.Messages.DuplicateData,
                };
            if (_bookRepository.GetBookAsync(book.Id).Result == null)
                return new ResultSet<BookInfo>() { IsSucceed = false, Message = Constants.Messages.BookNotFound, Data = book };


            if (!_bookRepository.EditBook(book))
                return new ResultSet<BookInfo>() { IsSucceed = false, Message = Constants.Messages.BookNotEdited, Data=book};

            try
            {
                await _bookRepository.SaveAsync();
            }
            catch (Exception e)
            {
                return new ResultSet<BookInfo>() { IsSucceed = false, Message = e.Message,Data=null };
            }
            return new ResultSet<BookInfo>() { IsSucceed = true, Message = string.Empty,Data=book };
        }

        public async Task<ResultSet> DeleteBookAsync(int bookId)
        {


            BookInfo book= await _bookRepository.GetBookAsync(bookId);
            if(book==null)
            {
                return new ResultSet() { IsSucceed = false, Message=Constants.Messages.BookNotFound };
            }
            if (!_bookRepository.DeleteBook(bookId))
                return new ResultSet() { IsSucceed = false, Message = Constants.Messages.BookNotDeleted };

            try
            {
                await _bookRepository.SaveAsync();
            }
            catch
            {
                return new ResultSet() { IsSucceed = false, Message = Constants.Messages.BookNotDeleted  };
            }
            return new ResultSet() { IsSucceed = true, Message = string.Empty };
        }

        public async Task<ResultSet<BookInfo>> GetBookAsync(int bookId)
        {
            BookInfo book = await _bookRepository.GetBookAsync(bookId);

            if (book == null)
                return new ResultSet<BookInfo>()
                {
                    IsSucceed = false,
                    Message = Constants.Messages.BookNotFound,
                };

            return new ResultSet<BookInfo>()
            {
                IsSucceed = true,
                Message = string.Empty,
                Data = book
            };

        }
    }
}
