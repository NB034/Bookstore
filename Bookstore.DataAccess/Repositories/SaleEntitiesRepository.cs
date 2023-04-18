using Bookstore.DataAccess.Contexts;
using Bookstore.DataAccess.Entities;

namespace Bookstore.DataAccess.Repositories
{
    internal class SaleEntitiesRepository : IRepository<SaleEntity>
    {
        private readonly BookstoreDbContext _context;

        public SaleEntitiesRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(SaleEntity entity)
        {
            _context.SaleEntities.Add(entity);
        }

        public void DeleteEntity(int id)
        {
            var entity = _context.SaleEntities.FirstOrDefault(e => e.Id == id);
            if (entity == null) throw new ArgumentException($"Entity with id {id} is not exist");
            _context.SaleEntities.Remove(entity);
        }

        public SaleEntity GetEntity(int id)
        {
            return _context.SaleEntities.FirstOrDefault(e => e.Id == id);
        }

        public void UpdateEntity(SaleEntity entity)
        {
            var sale = _context.SaleEntities.FirstOrDefault(e => e.Id == entity.Id);
            if (sale == null) throw new ArgumentException($"Entity with id {entity.Id} is not exist");

            sale.BookId = entity.BookId;
            sale.Date = entity.Date;
            sale.HowManySold = entity.HowManySold;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
