using Book.Store.Interfaces;
using Book.Store.Models.DM;
using System.Xml.Serialization;

namespace Book.Store.Services
{
    public class BooksService : IBooksService, IDisposable
    {
        private Bookstore _bookstore;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<BooksService> _logger;
        private readonly string _bookStoreXmlPath;

        public BooksService(IWebHostEnvironment env, ILogger<BooksService> logger)
        {
            _logger = logger;
            _env = env;
            _bookstore = new Models.DM.Bookstore();
            _bookStoreXmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", _env.EnvironmentName, "bookstore.xml");
            Init();
        }
        public bool AddBook(Models.DM.Book book)
        {
            _logger.LogInformation("{Method} has started, creating a new book with isdb: {Isbn}", nameof(UpdateBook), book?.Isbn);
            if (_bookstore == null)
            {
                _bookstore = new Bookstore();
            }

            if (book?.Valid == true)
            {
                var exists = _bookstore.Books.Any(b => b.Isbn == book.Isbn);
                if (!exists)
                {
                    _bookstore.Books.Add(book);
                    Flush();
                    return true;
                }
            }
            _logger.LogWarning("{Method} book model is not valid", nameof(AddBook));
            return false;
        }

        public bool DeleteBook(string isbn)
        {
            _logger.LogInformation("{Method} has started with isbn {Isbn}", nameof(DeleteBook), isbn);
            if (!string.IsNullOrWhiteSpace(isbn))
            {
                var book = _bookstore.Books.FirstOrDefault(b => b.Isbn == isbn);
                if (book != null)
                {
                    _bookstore.Books.Remove(book);
                    Flush();
                    return true;
                }
            }
            return false;
        }

        public bool UpdateBook(Models.DM.Book book)
        {
            _logger.LogInformation("{Method} has started, searching a book with isdb: {Isbn}", nameof(UpdateBook), book?.Isbn);
            var existingBook = _bookstore.Books.FirstOrDefault(b => b.Isbn == book.Isbn);
            if (existingBook != null)
            {
                _logger.LogInformation("{Method} book found! update", nameof(UpdateBook));
                if (book?.Valid == true)
                {
                    if (!string.IsNullOrWhiteSpace(book.Category) && book.Category != "string")
                    {
                        existingBook.Category = book.Category;
                    }
                    if (book?.Title != null && book.Title != "string")
                    {
                        existingBook.Title = book.Title;
                    }
                    if (book?.Authors?.Count > 0 && book.Authors[0] != "string")
                    {
                        existingBook.Authors = book.Authors;
                    }
                    if (book?.Year > 0)
                    {
                        existingBook.Year = book.Year;
                    }
                    if (book?.Price > 0)
                    {
                        existingBook.Price = book.Price;
                    }
                    Flush();
                    return true;
                }
            }
            _logger.LogWarning("{Method} book with isbn {Isbn} is not exist, abort", nameof(UpdateBook), book?.Isbn);
            return false;
        }

        private void Init()
        {
            _logger.LogInformation("{Method} started, book service xml path: {Path}", nameof(Init), _bookStoreXmlPath);
            _bookstore = new Bookstore();

            if (File.Exists(_bookStoreXmlPath))
            {
                _logger.LogInformation("{Method} book store xml file is exist, reading books", nameof(Init));
                var serializer = new XmlSerializer(typeof(Bookstore));
                using var reader = new StreamReader(_bookStoreXmlPath);
                _bookstore = (serializer.Deserialize(reader) as Bookstore) ?? new();
            }
        }

        private bool Flush()
        {
            _logger.LogInformation("{Method} flush has started, writing books to disk", nameof(Flush));
            var directory = Path.GetDirectoryName(_bookStoreXmlPath) ?? string.Empty;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);   
            }
            var serializer = new XmlSerializer(typeof(Bookstore));
            using var writer = new StreamWriter(_bookStoreXmlPath);
            serializer.Serialize(writer, _bookstore);
            return true;
        }


        public List<Models.DM.Book> GetBooks()
        {
            return _bookstore?.Books ?? new List<Models.DM.Book>();
        }

        public void Dispose()
        {
            if (_bookstore != null)
            {
                Flush();
            }
        }
    }
}
