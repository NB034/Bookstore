
namespace Bookstore.DataAccess.Entities
{
    internal class GenreEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}
