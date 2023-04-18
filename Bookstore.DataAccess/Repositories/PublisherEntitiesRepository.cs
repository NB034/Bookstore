using Bookstore.DataAccess.Contexts;
using Bookstore.DataAccess.Entities;

namespace Bookstore.DataAccess.Repositories
{
    internal class PublisherEntitiesRepository : IRepository<PublisherEntity>
    {
        private readonly BookstoreDbContext _context;

        public PublisherEntitiesRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(PublisherEntity entity)
        {
            _context.PublisherEntities.Add(entity);
        }

        public void DeleteEntity(int id)
        {
            var entity = _context.PublisherEntities.FirstOrDefault(e => e.Id == id);
            if (entity == null) throw new ArgumentException($"Entity with id {id} is not exist");
            _context.PublisherEntities.Remove(entity);
        }

        public PublisherEntity GetEntity(int id)
        {
            return _context.PublisherEntities.FirstOrDefault(e => e.Id == id);
        }

        public void UpdateEntity(PublisherEntity entity)
        {
            var publisher = _context.PublisherEntities.FirstOrDefault(e => e.Id == entity.Id);
            if (publisher == null) throw new ArgumentException($"Entity with id {entity.Id} is not exist");

            publisher.Name = entity.Name;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
