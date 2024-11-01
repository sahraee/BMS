using Microsoft.AspNetCore.Mvc;
using BMS.Application.Interfaces;
using BMS.Domain.Models.Book;
using BMS.DTO.DTOModels.Result;
using BMS.DTO.DTOModels.Book;
 

namespace BMS.Api.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;

        }

        /// <summary>
        /// This method returned information of a book
        /// </summary>
        /// <param name="bookId">Book identity</param>
        /// <returns>Information of a book</returns>
        /// <response code="200">Book detail</response>
        /// <response code="400">Book not found or parameter incorrect</response>
        [HttpGet("{bookId}")]
        public async Task<ActionResult<ResultSetDto<BookDetailDtoModel>>> GetBook(string bookId)
        {
            try
            {
                if (!int.TryParse(bookId, out int bid))
                {
                    throw new Exception(BMS.Domain.Constants.Messages.BookIdType);
                }

                var book = (await _bookService.GetBookAsync(bid)).Data;
                if (book == null)
                    return NotFound(new ResultSetDto<BookDetailDtoModel>()
                    {
                        IsSucceed = false,
                        Message = BMS.Domain.Constants.Messages.BookNotFound
                    });


                var result = new BookDetailDtoModel()
                {
                    BookId = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    PublishedYear = book.PublishedYear,
                    Genre = book.Genre
                };

                return Ok(new ResultSetDto<BookDetailDtoModel>()
                {
                    IsSucceed = true,
                    Message = "",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultSetDto<BookDetailDtoModel>()
                {
                    IsSucceed = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// This method edits the information of a book
        /// </summary>
        /// <param name="request">Data is a type of BookEditDtoModel object</param>
        /// <returns>The edited book information is returned</returns>
        /// <response code="200">Book detail</response>
        /// <response code="400">Book not found or parameters incorrect</response>
        [HttpPut]
        public async Task<ActionResult<ResultSetDto<BookEditDtoModel>>> EditBook([FromBody] BookEditDtoModel request)
        {
            if (!ModelState.IsValid)
            {
                string message = "";
                foreach (var er in ModelState.Values.SelectMany(modelstate => modelstate.Errors))
                    message += er.ErrorMessage + " \n";

                return BadRequest(new ResultSetDto()
                {
                    IsSucceed = false,
                    Message = message
                });
            }

            try
            {

                var resultBook = await _bookService.GetBookAsync(request.BookId);

                if (resultBook == null || resultBook.Data == null || !resultBook.IsSucceed)
                    return BadRequest(new ResultSetDto<BookEditDtoModel>()
                    {
                        IsSucceed = false,
                        Message = resultBook.Message
                    });

                BookInfo bookEdit = resultBook.Data;
                bookEdit.Title = request.Title;
                bookEdit.PublishedYear = request.PublishedYear;
                bookEdit.Genre = request.Genre;
                bookEdit.Author = request.Author;

                var result = await _bookService.EditBookAsync(bookEdit);
                if (!result.IsSucceed)
                    return BadRequest(new ResultSetDto<BookEditDtoModel>()
                    {
                        IsSucceed = false,
                        Message = result.Message
                    });

                return Ok(new ResultSetDto<BookEditDtoModel>()
                {
                    IsSucceed = true,
                    Message = "",
                    Data = request
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultSetDto<BookEditDtoModel>()
                {
                    IsSucceed = false,
                    Message = ex.Message
                });
            }
        }
        /// <summary>
        /// This method adds the information of a book 
        /// </summary>
        /// <param name="request">Data is a type of BookNewDtoModel object</param>
        /// <returns>The added book information is returned</returns>
        /// <response code="200">Book detail</response>
        /// <response code="400">Book duplicate data or parameter incorrect</response>
        [HttpPost]
        public async Task<ActionResult<ResultSetDto<BookDetailDtoModel>>> AddBook([FromBody] BookNewDtoModel request)
        {
            if (!ModelState.IsValid)
            {
                string message = "";
                foreach (var er in ModelState.Values.SelectMany(modelstate => modelstate.Errors))
                    message += er.ErrorMessage + " \n";

                return BadRequest(new ResultSetDto()
                {
                    IsSucceed = false,
                    Message = message
                });
            }

            try
            {
                BookInfo book = new BookInfo()
                {
                    Title = request.Title,
                    PublishedYear = request.PublishedYear,
                    Author = request.Author,
                    Genre = request.Genre,

                };

                var resultSave = await _bookService.AddBookAsync(book);
                if (!resultSave.IsSucceed)
                    return BadRequest(new ResultSetDto<BookDetailDtoModel>()
                    {
                        IsSucceed = false,
                        Message = resultSave.Message,
                    });

                BookDetailDtoModel result = new BookDetailDtoModel()
                {
                    BookId = resultSave.Data.Id,
                    Title = request.Title,
                    PublishedYear = request.PublishedYear,
                    Author = request.Author,
                    Genre = request.Genre
                };

                return Ok(new ResultSetDto<BookDetailDtoModel>()
                {
                    IsSucceed = true,
                    Message = "",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultSetDto<BookDetailDtoModel>()
                {
                    IsSucceed = false,
                    Message = ex.Message,
                });
            }
        }

        /// <summary>
        /// This method delete a book
        /// </summary>
        /// <param name="bookId">Identity</param>
        /// <returns>Delete result returned</returns>
        /// <response code="200">true</response>
        /// <response code="400">Book not found or parameter incorrect</response>
        [HttpDelete]
        public async Task<ActionResult<ResultSetDto>> DeleteBook(string bookId)
        {
            if (!ModelState.IsValid)
            {
                string message = "";
                foreach (var er in ModelState.Values.SelectMany(modelstate => modelstate.Errors))
                    message += er.ErrorMessage + " \n";

                return BadRequest(new ResultSetDto()
                {
                    IsSucceed = false,
                    Message = message
                });
            }

            try
            {
                if (!int.TryParse(bookId, out int bid))
                {
                    throw new Exception(BMS.Domain.Constants.Messages.BookIdType);
                }

                var result = await _bookService.DeleteBookAsync(bid);

                if (!result.IsSucceed)
                    return BadRequest(new ResultSetDto()
                    {
                        IsSucceed = false,
                        Message = result.Message
                    });


                return Ok(new ResultSetDto()
                {
                    IsSucceed = true,
                    Message = ""
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultSetDto()
                {
                    IsSucceed = false,
                    Message = ex.Message

                });
            }
        }


    }
}

