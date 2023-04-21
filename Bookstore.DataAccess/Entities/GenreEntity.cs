
namespace Bookstore.DataAccess.Entities
{
    public class GenreEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}
