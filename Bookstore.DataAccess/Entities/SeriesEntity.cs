
namespace Bookstore.DataAccess.Entities
{
    public class SeriesEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}
