using Bookstore.DataAccess.Contexts;
using Bookstore.DataAccess.Entities;

namespace Bookstore.DataAccess.Repositories
{
    internal class AuthorEntitiesRepository : IRepository<AuthorEntity>
    {
        private readonly BookstoreDbContext _context;

        public AuthorEntitiesRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(AuthorEntity entity)
        {
            _context.AuthorEntities.Add(entity);
        }

        public void DeleteEntity(int id)
        {
            var entity = _context.AuthorEntities.FirstOrDefault(e => e.Id == id);
            if (entity == null) throw new ArgumentException($"Entity with id {id} is not exist");
            _context.AuthorEntities.Remove(entity);
        }

        public AuthorEntity GetEntity(int id)
        {
            return _context.AuthorEntities.FirstOrDefault(e => e.Id == id);
        }

        public void UpdateEntity(AuthorEntity entity)
        {
            var author = _context.AuthorEntities.FirstOrDefault(e => e.Id == entity.Id);
            if (author == null) throw new ArgumentException($"Entity with id {entity.Id} is not exist");

            author.Name = entity.Name;
            author.Surname = entity.Surname;
            author.Patronymic = entity.Patronymic;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
