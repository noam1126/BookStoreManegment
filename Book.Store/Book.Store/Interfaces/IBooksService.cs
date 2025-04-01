
using Book.Store.Models.DM;

namespace Book.Store.Interfaces
{
    public interface IBooksService
    {
        List<Models.DM.Book> GetBooks();
        bool AddBook(Models.DM.Book book);
        bool DeleteBook(string isbn);
        bool UpdateBook(Models.DM.Book book);
    }
}
