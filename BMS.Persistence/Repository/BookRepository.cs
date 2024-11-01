using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.Book;
using BMS.Persistence.Contexts;

namespace BMS.Persistence.Repository
{
    public class BookRepository : IBookRepository
    {

        private GenericRepository<BookInfo> _bookRepository;

        public BookRepository(BMSDBContext ctx)
        {
            this._bookRepository = new GenericRepository<BookInfo>(ctx);
        }

       
         
        public bool AddBook(BookInfo book)
        {
            try
            {
                _bookRepository.Insert(book);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool EditBook(BookInfo book)
        {
            try
            {
                
                _bookRepository.Update(book);
            }
            catch
            { 
                return false; 
            }
            return true;
        }

        public async Task<BookInfo> GetBookAsync(int bookId)
        {
            return await _bookRepository.GetByIdAsync(s=>s.Id==bookId);
        
        }
 
         
        public void Save()
        {
            _bookRepository.Save();
        }

        public async Task SaveAsync() =>
            await _bookRepository.SaveAsync();
 
        public bool IsBookExists(string title, int publishedYear, string author, string genre, int? id)
        {
            return (_bookRepository.Get(b => b.Title == title && b.PublishedYear==publishedYear && b.Author==author && b.Genre==genre && (id==null || b.Id!=id)).Count() > 0 ? true : false);
        }

        

        public  bool DeleteBook(int bookId)
        {
            var book = GetBook(bookId);
            if (book == null)
                return false;
            try
            {
                book.IsRemoved = true;
                book.RemoveDate = DateTime.Now;
                _bookRepository.Update(book);
            }
            catch
            {
                return false;
            }
            return true;
        }


        private BookInfo GetBook(int bookId)
        {
            return _bookRepository.GetById(b=>b.Id== bookId );
        }
    }
}
