
namespace Bookstore.ViewModels
{
    class BookViewModel
    {
        public BookViewModel(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Pages { get; set; }
        public string PublicationYear { get; set; }
        public string Quantity { get; set; }
        public string CostPrice { get; set; }
        public string SalePrice { get; set; }
        public string Genre { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string AuthorPatronymic { get; set; }
        public string Publisher { get; set; }
        public string Series { get; set; }
    }
}
