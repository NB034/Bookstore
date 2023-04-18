
namespace Bookstore.DataAccess.Entities
{
    internal class AuthorEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}
