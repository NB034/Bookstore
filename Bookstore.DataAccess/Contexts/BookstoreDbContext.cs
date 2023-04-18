using Bookstore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.DataAccess.Contexts
{
    internal class BookstoreDbContext : DbContext
    {
        public BookstoreDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<AuthorEntity> AuthorEntities { get; set; }
        public virtual DbSet<BookEntity> BookEntities { get; set; }
        public virtual DbSet<GenreEntity> GenreEntities { get; set; }
        public virtual DbSet<PublisherEntity> PublisherEntities { get; set; }
        public virtual DbSet<SaleEntity> SaleEntities { get; set; }
        public virtual DbSet<SeriesEntity> SeriesEntities { get; set; }
    }
}
