using Bookstore.DataAccess.Contexts;
using Bookstore.DataAccess.Entities;

namespace Bookstore.DataAccess.Repositories
{
    internal class SeriesEntitiesRepository : IRepository<SeriesEntity>
    {
        private readonly BookstoreDbContext _context;

        public SeriesEntitiesRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(SeriesEntity entity)
        {
            _context.SeriesEntities.Add(entity);
        }

        public void DeleteEntity(int id)
        {
            var entity = _context.SeriesEntities.FirstOrDefault(e => e.Id == id);
            if (entity == null) throw new ArgumentException($"Entity with id {id} is not exist");
            _context.SeriesEntities.Remove(entity);
        }

        public SeriesEntity GetEntity(int id)
        {
            return _context.SeriesEntities.FirstOrDefault(e => e.Id == id);
        }

        public void UpdateEntity(SeriesEntity entity)
        {
            var series = _context.SeriesEntities.FirstOrDefault(e => e.Id == entity.Id);
            if (series == null) throw new ArgumentException($"Entity with id {entity.Id} is not exist");

            series.Name = entity.Name;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
