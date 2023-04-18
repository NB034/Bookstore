
namespace Bookstore.DataAccess.Entities
{
    internal class SaleEntity
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int BookId { get; set; }
        public int HowManySold { get; set; }

        public BookEntity Book { get; set; }
    }
}
