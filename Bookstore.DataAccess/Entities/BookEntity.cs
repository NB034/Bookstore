
namespace Bookstore.DataAccess.Entities
{
    internal class BookEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public DateOnly PublicationYear { get; set; }
        public int Quantity { get; set; }
        public int CostPrice { get; set; }
        public int SalePrice { get; set; }

        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int SeriesId { get; set; }

        public GenreEntity Genre { get; set; }
        public AuthorEntity Author { get; set; }
        public PublisherEntity Publisher { get; set; }
        public SeriesEntity Series { get; set; }

        public virtual ICollection<SaleEntity> Sales { get; set; }
    }
}
