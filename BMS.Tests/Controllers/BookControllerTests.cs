using BMS.Application.Interfaces;
using Moq;
using BMS.Application.Services;
using BMS.Domain.Interfaces;
using BMS.Domain.Models.Book;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using Microsoft.AspNetCore.Mvc;
using BMS.DTO.DTOModels.Book;
using BMS.DTO.DTOModels.Result;
 
namespace BMS.Api.Controllers.Tests
{
    [TestClass()]
    public class BookControllerTests
    {
        IBookService _bookService;
        IBookRepository _bookRepository;

        List<BookInfo> _testBooks;

        public BookControllerTests()
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

            _bookService = new BookService(_bookRepository);

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

                },

                new BookInfo()
                {
                    Author="Test1",
                    Genre="Drama",
                    Id=2,
                     PublishedYear=2010,
                      Title="Book name 2",

                }

                ,
                new BookInfo()
                {
                    Author="Test2",
                    Genre="Drama",
                    Id=4,
                     PublishedYear=2022,
                      Title="Book name 3",

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
                  addedBook.Id = newId;      

                 _testBooks.Add(addedBook);

             })).Returns(true);
            mockBookRepository.Setup(br => br.GetBookAsync(It.IsAny<int>()))
                .ReturnsAsync(new Func<int, BookInfo>(
                    id => _testBooks.Find(br => br.Id.Equals(id))));

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
                        var _bookRemove = _testBooks.Find(b => b.Id == x);

                        if (_bookRemove != null)
                            _testBooks.Remove(_bookRemove);
                    })).Returns(true);


            mockBookRepository.Setup(x => x.IsBookExists(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>()))
    .Returns<string, int, string, string, int?>((title, year, author, genre, id) =>

        _testBooks.Count(book => (book.Author == author) && (book.Genre == genre) && (book.Title == title) && (book.PublishedYear == year) && (id == null || book.Id != id)) > 0

    );


            return mockBookRepository.Object;

        }
        #endregion

        #region Test methods

        [TestMethod()]
        public void GetBookTest()
        {

            var _bookController = new BookController(_bookService);
            var result = _bookController.GetBook("1").Result;

            Assert.That(result != null);
            Assert.That(result.Result is OkObjectResult);
            Assert.That(((ResultSetDto<BookDetailDtoModel>)((OkObjectResult)result.Result).Value).Data.Title == "Book name 1");
        }

        [TestMethod()]

        public void EditBookTest()
        {

            var _bookController = new BookController(_bookService);
            var result = _bookController.EditBook(new DTO.DTOModels.Book.BookEditDtoModel()
            {
                Author = "New 1",
                Genre = "Fiction",
                PublishedYear = DateTime.Now.Year,
                Title = "New book name",
                BookId = 2
            }).Result;
            Assert.That(result != null);
            Assert.That(result.Result is OkObjectResult);
            Assert.That(((ResultSetDto<BookEditDtoModel>)((OkObjectResult)result.Result).Value).Data.Title == "New book name");

        }
        [TestMethod]
        public void EditBookDuplicateTest()
        {

            var _bookController = new BookController(_bookService);
            var result = _bookController.EditBook(new DTO.DTOModels.Book.BookEditDtoModel()
            {
                Author = "Test1",
                Genre = "Drama",
                PublishedYear = 2010,
                Title = "Book name 1",
                BookId = 2
            }).Result;
            Assert.That(result != null);
            Assert.That(result.Result is BadRequestObjectResult);
            Assert.That(((ResultSetDto<BookEditDtoModel>)((BadRequestObjectResult)result.Result).Value).Data == null);
            Assert.That(((ResultSetDto<BookEditDtoModel>)((BadRequestObjectResult)result.Result).Value).Message == BMS.Domain.Constants.Messages.DuplicateData);

        }
        [TestMethod()]
        public void AddBookTest()

        {
            var _bookController = new BookController(_bookService);
            var result = _bookController.AddBook(new DTO.DTOModels.Book.BookNewDtoModel()
            {
                Author = "New 1",
                Genre = "Fiction",
                PublishedYear = DateTime.Now.Year,
                Title = "New book"
            }).Result;
            Assert.That(result != null);
            Assert.That(result.Result is OkObjectResult);
            Assert.That(((ResultSetDto<BookDetailDtoModel>)((OkObjectResult)result.Result).Value).Data.BookId == 5);


        }


        [TestMethod]
        public void AddDuplicteBookTest()

        {
            var _bookController = new BookController(_bookService);
            BookNewDtoModel newBook = new BookNewDtoModel()
            {
                Author = "New 1",
                Genre = "Fiction",
                PublishedYear = DateTime.Now.Year,
                Title = "New book"
            };
            var result = _bookController.AddBook(newBook).Result;
            result = _bookController.AddBook(newBook).Result;
            Assert.That(result != null);
            Assert.That(result.Result is BadRequestObjectResult);
            Assert.That(((ResultSetDto<BookDetailDtoModel>)((BadRequestObjectResult)result.Result).Value).Data == null);
            Assert.That(((ResultSetDto<BookDetailDtoModel>)((BadRequestObjectResult)result.Result).Value).Message == BMS.Domain.Constants.Messages.DuplicateData);



        }
        [TestMethod()]
        public void DeleteBookWithWrongIdTest()
        {

            var _bookController = new BookController(_bookService);
            var result = _bookController.DeleteBook("1K").Result;
            Assert.That(result != null);
            Assert.That(result.Result is BadRequestObjectResult);
            Assert.That(((ResultSetDto)((BadRequestObjectResult)result.Result).Value).Message == BMS.Domain.Constants.Messages.BookIdType);

        }

        [TestMethod()]
        public void DeleteBookTest()
        {
            var _bookController = new BookController(_bookService);
            var result = _bookController.DeleteBook("1").Result;
            Assert.That(result != null);
            Assert.That(result.Result is OkObjectResult);
            Assert.That(((ResultSetDto)((OkObjectResult)result.Result).Value).IsSucceed == true);
        }

        [TestMethod()]
        public void DeleteBookWithWrongIdTest2()
        {
            var _bookController = new BookController(_bookService);
            var result = _bookController.DeleteBook("21").Result;
            Assert.That(result != null);
            Assert.That(result.Result is BadRequestObjectResult);
            Assert.That(((ResultSetDto)((BadRequestObjectResult)result.Result).Value).Message == BMS.Domain.Constants.Messages.BookNotFound);


        }
        #endregion
    }
}