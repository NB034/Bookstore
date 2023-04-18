
namespace Bookstore.DataAccess.Entities
{
    internal class SeriesEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}
