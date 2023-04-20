using Bookstore.ViewModels;

namespace Bookstore.Models
{
    interface IBookstoreModel
    {
        BookViewModel[] GetBooks();
        void AddBook(BookViewModel book);
        void UpdateBook(BookViewModel book);
        void DeleteBook(int id);
    }
}
