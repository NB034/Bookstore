using Bookstore.DataAccess.Contexts;
using Bookstore.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.DataAccess.Repositories
{
    internal class GenreEntitiesPepository : IRepository<GenreEntity>
    {
        private readonly BookstoreDbContext _context;

        public GenreEntitiesPepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(GenreEntity entity)
        {
            _context.GenreEntities.Add(entity);
        }

        public void DeleteEntity(int id)
        {
            var entity = _context.GenreEntities.FirstOrDefault(e => e.Id == id);
            if (entity == null) throw new ArgumentException($"Entity with id {id} is not exist");
            _context.GenreEntities.Remove(entity);
        }

        public GenreEntity GetEntity(int id)
        {
            return _context.GenreEntities.FirstOrDefault(e => e.Id == id);
        }

        public void UpdateEntity(GenreEntity entity)
        {
            var genre = _context.GenreEntities.FirstOrDefault(e => e.Id == entity.Id);
            if (genre == null) throw new ArgumentException($"Entity with id {entity.Id} is not exist");

            genre.Name = entity.Name;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
