using Bookstore.ViewModels;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    interface IBookstoreModel
    {
        Task<BookViewModel[]> GetBooks();
        Task AddBook(BookViewModel book);
        Task UpdateBook(BookViewModel book);
        Task DeleteBook(int id);
    }
}
