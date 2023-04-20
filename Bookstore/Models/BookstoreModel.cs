using Bookstore.ViewModels;
using System;
using System.Collections.Generic;
using Bookstore.DataAccess.Contexts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    class BookstoreModel : IBookstoreModel
    {
        private readonly BookstoreDbContext _dbContext;

        public void AddBook(BookViewModel book)
        {
            throw new NotImplementedException();
        }

        public void DeleteBook(int id)
        {
            throw new NotImplementedException();
        }

        public BookViewModel[] GetBooks()
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(BookViewModel book)
        {
            throw new NotImplementedException();
        }
    }
}
