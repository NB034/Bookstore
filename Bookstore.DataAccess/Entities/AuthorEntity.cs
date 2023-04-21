
namespace Bookstore.DataAccess.Entities
{
    public class AuthorEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}
