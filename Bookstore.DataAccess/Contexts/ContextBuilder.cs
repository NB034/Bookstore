using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.DataAccess.Contexts
{
    public static class ContextBuilder
    {
        public static BookstoreDbContext GetBookstoreDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookstoreDbContext>();
            var options = optionsBuilder.UseSqlite(connectionString).Options;
            return new BookstoreDbContext(options);
        }
    }
}
