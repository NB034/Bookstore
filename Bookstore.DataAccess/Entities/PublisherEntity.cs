
namespace Bookstore.DataAccess.Entities
{
    internal class PublisherEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BookEntity> Books { get; set; }
    }
}
