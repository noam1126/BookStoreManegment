using Book.Store.Interfaces;
using Book.Store.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book.Store.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBooksService booksService, ILogger<BooksController> logger)
        {
            _booksService = booksService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            try
            {
                var books = _booksService.GetBooks()?.Select(book => book.ToDTO())?.ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} an exception was thrown", nameof(GetBooks));
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Models.DTO.Book newBook)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("{Method} add book, model is not valid", nameof(AddBook));
                return BadRequest("Invalid data.");
            }
            try
            {
                var result = _booksService.AddBook(newBook.ToDataModel());
                if (result)
                {
                    return Ok(newBook);
                }
                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} an exception was thrown", nameof(GetBooks));
                return StatusCode(500);
            }
        }

        [HttpPut("{isbn}")]
        public IActionResult UpdateBook(string isbn, [FromBody] Models.DTO.Book book)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("{Method} model is not valid", nameof(UpdateBook));
                return BadRequest();
            }
            try
            {
                book.Isbn = isbn;
                var result = _booksService.UpdateBook(book.ToDataModel());
                if (result)
                {
                    return Ok();
                }

                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} an exception was thrown", nameof(GetBooks));
                return StatusCode(500);
            }
        }

        [HttpDelete("{isbn}")]
        public IActionResult DeleteBook(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                _logger.LogWarning("{Method} isbn is null or empty", nameof(DeleteBook));
                return BadRequest();
            }

            try
            {
                var result = _booksService.DeleteBook(isbn);
                if (result)
                {
                    return Ok(); // Successfully deleted
                }
                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} an exception was thrown", nameof(GetBooks));
                return StatusCode(500);
            }
        }
    }

}
