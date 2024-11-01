using BMS.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BMS.Application.Interfaces;
using BMS.Api.Controllers;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.Book;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using BMS.DTO.DTOModels.Book;
using BMS.DTO.DTOModels.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using BMS.Domain;

namespace BMS.Tests
{
    [TestClass()]
    public class BookServiceTests
    {
        IBookRepository _bookRepository;
        List<BookInfo> _testBooks;

        public BookServiceTests()
        {
            Setup();
            SetupBooks();
            SetupRepository();

        }
        #region Setup methods
        [SetUp]
        public void Setup()
        {
            _testBooks = SetupBooks();
            _bookRepository = SetupRepository();


        }
        public List<BookInfo> SetupBooks()
        {
            List<BookInfo> books = new List<BookInfo> {

                new BookInfo()
                {
                    Author="Test1",
                    Genre="Drama",
                    Id=1,
                    PublishedYear=2010,
                    Title="Book name 1",
                    IsRemoved = false,
                    RegDate=DateTime.Now,
                    RemoveDate=null,
                    UpdateDate=null,

                },

                new BookInfo()
                {
                    Author="Test Author2",
                    Genre="Drama",
                    Id=2,
                    PublishedYear=2010,
                    Title="Book name 2",
                    IsRemoved = false,
                    RegDate=DateTime.Now,
                    RemoveDate=null,
                    UpdateDate=null,

                }

                ,
                 new BookInfo()
                {
                    Author="Test Author2",
                    Genre="Drama",
                    Id=3,
                    PublishedYear=2010,
                    Title="Book name 3",
                    IsRemoved = true,
                    RegDate=DateTime.Now,
                    RemoveDate=DateTime.Now,
                    UpdateDate=null,

                }

                ,

                new BookInfo()
                {
                    Author="Test2",
                    Genre="Drama",
                    Id=4,
                    PublishedYear=2022,
                    Title="Book name 3",
                    IsRemoved = false,
                    RegDate=DateTime.Now,
                    RemoveDate=null,
                    UpdateDate=null,
                    
                }

            };
            return books;
        }

        public IBookRepository SetupRepository()
        {


            var mockBookRepository = new Mock<IBookRepository>();
            mockBookRepository.Setup(br => br.AddBook(It.IsAny<BookInfo>()))
             .Callback(new Action<BookInfo>(addedBook =>
             {
                 dynamic maxId = _testBooks.Max(b => b.Id);
                 dynamic newId = maxId + 1;
                 BookInfo newBook = new BookInfo();
                 addedBook.Id = newId;
                 newBook.Id = newId;
                 newBook.Author = addedBook.Author;
                 newBook.Title = addedBook.Title;
                 newBook.Genre = addedBook.Genre;
                 newBook.PublishedYear = addedBook.PublishedYear;

                 _testBooks.Add(newBook);

             })).Returns(true);
            mockBookRepository.Setup(br => br.GetBookAsync(It.IsAny<int>()))
                .ReturnsAsync(new Func<int, BookInfo>(
                    id => _testBooks.Find(br => br.Id.Equals(id) && br.IsRemoved==false)));

            mockBookRepository.Setup(br => br.EditBook(It.IsAny<BookInfo>()))
                .Callback(new Action<BookInfo>(editedBook =>
                {
                    var oldBook = _testBooks.Find(b => b.Id == editedBook.Id);
                    oldBook.UpdateDate = DateTime.Now;
                    oldBook.Author = editedBook.Author;
                    oldBook.Title = editedBook.Title;
                    oldBook.Genre = editedBook.Genre;
                    oldBook.PublishedYear = editedBook.PublishedYear;
                })).Returns(true);

            mockBookRepository.Setup(br => br.DeleteBook(It.IsAny<int>()))
                    .Callback(new Action<int>(x =>
                    {
                        var bookRemove = _testBooks.Find(b => b.Id == x && b.IsRemoved==false);

                        if (bookRemove != null)
                            bookRemove.IsRemoved = true;
                            //_testBooks.Remove(_bookRemove);
                    })).Returns(true);


            mockBookRepository.Setup(x => x.IsBookExists(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
             .Returns<string, int, string, string, int?>((title, year, author, genre, id) =>
              _testBooks.Count(book => book.IsRemoved==false && (book.Author == author) && (book.Genre == genre) && (book.Title == title) &&
              (book.PublishedYear == year) && (id == null || book.Id != id)) > 0);


            return mockBookRepository.Object;

        }
        #endregion




        [TestMethod()]
        public  void AddBookAsyncTest()
        {

            var bookService = new BookService(_bookRepository);
            var result =  bookService.AddBookAsync(new BookInfo()
            {
                Id = 0,
                Author = "New 1",
                Genre = "Fiction",
                PublishedYear = DateTime.Now.Year,
                Title = "New book",
                IsRemoved = false,
                RegDate = DateTime.Now,
                RemoveDate = null,
                UpdateDate = null
            }).Result;
            Assert.That(result != null);
            Assert.That(result.IsSucceed);
            Assert.That(result.Data != null);
            Assert.That(result.Data.Id == 5);
        }

        [TestMethod()]
        public void AddDuplicateBookTest()
        {

            var bookService = new BookService(_bookRepository);
            BookInfo book = new BookInfo()
            {
                Id = 0,
                Author = "New 1",
                Genre = "Fiction",
                PublishedYear = DateTime.Now.Year,
                Title = "New book",
                IsRemoved = false,
                RegDate = DateTime.Now,
                RemoveDate = null,
                UpdateDate = null
            };

            var result = bookService.AddBookAsync(book).Result;
            result = bookService.AddBookAsync(book).Result;
            Assert.That(result != null);
            Assert.That(!result.IsSucceed);
            Assert.That(result.Message == Constants.Messages.DuplicateData);
        }


        [TestMethod()]
        public void EditBookAsyncTest()
        {

            var bookService = new BookService(_bookRepository);
            var result = bookService.EditBookAsync(new BookInfo()
            {
                Id = 1,
                Author = "New1",
                Genre = "Fiction",
                PublishedYear = DateTime.Now.Year,
                Title = "Edit book",
                IsRemoved = false,
                RegDate = DateTime.Now,
                RemoveDate = null,
                UpdateDate = DateTime.Now
            }).Result;
            Assert.That(result != null);
            Assert.That(result.IsSucceed);
            Assert.That(result.Data != null);
            Assert.That(result.Data.Title == "Edit book" && result.Data.UpdateDate.HasValue && result.Data.Id==1);
        }

        [TestMethod()]
        public void EditNoneExistsBookAsyncTest()
        {

            var bookService = new BookService(_bookRepository);
            var result = bookService.EditBookAsync(new BookInfo()
            {
                Id = 100,
                Author = "New1",
                Genre = "Fiction",
                PublishedYear = DateTime.Now.Year,
                Title = "Edit book",
                IsRemoved = false,
                RegDate = DateTime.Now,
                RemoveDate = null,
                UpdateDate = DateTime.Now
            }).Result;
            Assert.That(result != null);
            Assert.That(result.Data!=null);
            Assert.That(!result.IsSucceed);
            Assert.That(result.Message == Constants.Messages.BookNotFound);
        }

        [TestMethod()]
        public void DeleteBookAsyncTest()
        {

            var bookService = new BookService(_bookRepository);
            var result = bookService.DeleteBookAsync(1).Result;
            Assert.That(result != null);
            Assert.That(result.IsSucceed);
            Assert.That(_testBooks.Find(b=>b.Id==1).IsRemoved==true);


        }


        [TestMethod()]
        public void DeleteNoneExistsBookAsyncTest()
        {

            var bookService = new BookService(_bookRepository);
            var result = bookService.DeleteBookAsync(100).Result;
            Assert.That(result != null);
            Assert.That(!result.IsSucceed);
            Assert.That(result.Message== Constants.Messages.BookNotFound);


        }


        [TestMethod()]
        public void GetBookAsyncTest()
        {

            var bookService = new BookService(_bookRepository);
            var result = bookService.GetBookAsync(1).Result;

            Assert.That(result != null);
            Assert.That(result.IsSucceed);
            Assert.That(result.Data.Id==1);
        }
        [TestMethod()]
        public void GetNoneExsitsBookAsyncTest()
        {

            var bookService = new BookService(_bookRepository);
            var result = bookService.GetBookAsync(11).Result;

            Assert.That(result != null);
            Assert.That(!result.IsSucceed);
            Assert.That(result.Message == Constants.Messages.BookNotFound);

        }

    }
}