using Bookstore.ViewModels;
using System.Windows;

namespace Bookstore
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new BookstoreViewModel();
        }
    }
}
