using Bookstore.DataAccess.Contexts;
using Bookstore.DataAccess.Entities;

namespace Bookstore.DataAccess.Repositories
{
    internal class BookEntitiesRepository : IRepository<BookEntity>
    {
        private readonly BookstoreDbContext _context;

        public BookEntitiesRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(BookEntity entity)
        {
            _context.BookEntities.Add(entity);
        }

        public void DeleteEntity(int id)
        {
            var entity = _context.BookEntities.FirstOrDefault(e => e.Id == id);
            if (entity == null) throw new ArgumentException($"Entity with id {id} is not exist");
            _context.BookEntities.Remove(entity);
        }

        public BookEntity GetEntity(int id)
        {
            return _context.BookEntities.FirstOrDefault(e => e.Id == id);
        }

        public void UpdateEntity(BookEntity entity)
        {
            var book = _context.BookEntities.FirstOrDefault(e => e.Id == entity.Id);
            if (book == null) throw new ArgumentException($"Entity with id {entity.Id} is not exist");

            book.Title = entity.Title;
            book.Description = entity.Description;
            book.Pages = entity.Pages;
            book.PublicationYear = entity.PublicationYear;
            book.Quantity = entity.Quantity;
            book.CostPrice = entity.CostPrice;
            book.SalePrice = entity.SalePrice;

            book.GenreId = entity.GenreId;
            book.AuthorId = entity.AuthorId;
            book.PublisherId = entity.PublisherId;
            book.SeriesId = entity.SeriesId;
                    }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
