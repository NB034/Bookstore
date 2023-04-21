
namespace Bookstore.DataAccess.Entities
{
    public class PublisherEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}
