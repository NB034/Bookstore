﻿using Bookstore.Command;
using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Bookstore.ViewModels
{
    internal partial class BookstoreViewModel : INotifyPropertyChanged
    {
        private readonly IBookstoreModel _model;

        private BookViewModel _selectedBook = null;
        private string _title = "";
        private string _description = "";
        private string _pages = "";
        private string _publicationYear = "";
        private string _quantity = "";
        private string _costPrice = "";
        private string _salePrice = "";
        private string _genre = "";
        private string _authorName = "";
        private string _authorSurname = "";
        private string _authorPatronymic = "";
        private string _publisher = "";
        private string _series = "";

        private List<BookViewModel> _books;
        private bool IsBookSelected => _selectedBook != null;

        private bool IsRequiredFieldsEmpty => _title == String.Empty
            && _publicationYear == String.Empty
            && _genre == String.Empty
            && _authorName == String.Empty
            && _authorSurname == String.Empty
            && _publisher == String.Empty;

        private bool IsNumericFieldsCorrect => int.TryParse(_pages, out _)
            && int.TryParse(_publicationYear, out _)
            && int.TryParse(_quantity, out _)
            && int.TryParse(_costPrice, out _)
            && int.TryParse(_salePrice, out _);

        private bool WereFieldChanged => IsBookSelected && !(_selectedBook.Title == _title
            && _selectedBook.Description == _description
            && _selectedBook.Pages == _pages
            && _selectedBook.PublicationYear == _publicationYear
            && _selectedBook.Quantity == _quantity
            && _selectedBook.CostPrice == _costPrice
            && _selectedBook.SalePrice == _salePrice
            && _selectedBook.Genre == _genre
            && _selectedBook.AuthorName == _authorName
            && _selectedBook.AuthorSurname == _authorSurname
            && _selectedBook.AuthorPatronymic == _authorPatronymic
            && _selectedBook.Publisher == _publisher
            && _selectedBook.Series == _series);

        public event PropertyChangedEventHandler PropertyChanged;

        public BookstoreViewModel()
        {
            _model = new BookstoreModel();

            ComboBoxItems = new List<string>
            {
                "Title",
                "Author",
                "Genre"
            };

            Books = new List<BookViewModel>(_model.GetBooks());

            _searchCommand = new AutoEventCommandBase(_ => Search(), _ => true);
            _resetCommand = new AutoEventCommandBase(_ => Reset(), _ => true);
            _saveCommand = new AutoEventCommandBase(_ => Save(), _ => CanSave());
            _createCommand = new AutoEventCommandBase(_ => Create(), _ => CanCreate());
            _cancelCommand = new AutoEventCommandBase(_ => Cancel(), _ => CanCancel());
            _clearCommand = new AutoEventCommandBase(_ => Clear(), _ => CanClear());
            _deleteCommand = new AutoEventCommandBase(_ => Delete(), _ => CanDelete());

            PropertyChanged += OnSelectionChange;
        }

        public BookViewModel SelectedBook { get => _selectedBook; set => SetProperty(ref _selectedBook, value, nameof(SelectedBook)); }
        public string Title { get => _title; set => SetProperty(ref _title, value, nameof(Title)); }
        public string Description { get => _description; set => SetProperty(ref _description, value, nameof(Description)); }
        public string Pages { get => _pages; set => SetProperty(ref _pages, value, nameof(Pages)); }
        public string PublicationYear { get => _publicationYear; set => SetProperty(ref _publicationYear, value, nameof(PublicationYear)); }
        public string Quantity { get => _quantity; set => SetProperty(ref _quantity, value, nameof(Quantity)); }
        public string CostPrice { get => _costPrice; set => SetProperty(ref _costPrice, value, nameof(CostPrice)); }
        public string SalePrice { get => _salePrice; set => SetProperty(ref _salePrice, value, nameof(SalePrice)); }
        public string Genre { get => _genre; set => SetProperty(ref _genre, value, nameof(Genre)); }
        public string AuthorName { get => _authorName; set => SetProperty(ref _authorName, value, nameof(AuthorName)); }
        public string AuthorSurname { get => _authorSurname; set => SetProperty(ref _authorSurname, value, nameof(AuthorSurname)); }
        public string AuthorPatronymic { get => _authorPatronymic; set => SetProperty(ref _authorPatronymic, value, nameof(AuthorPatronymic)); }
        public string Publisher { get => _publisher; set => SetProperty(ref _publisher, value, nameof(Publisher)); }
        public string Series { get => _series; set => SetProperty(ref _series, value, nameof(Series)); }

        public List<BookViewModel> Books { get => _books; set => SetProperty(ref _books, value, nameof(Books)); }

        public List<string> ComboBoxItems { get; }
        public string SelectedItem { get; set; }
        public string SearchTextBox { get; set; }

        private void OnSelectionChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SelectedBook)) return;
            if (SelectedBook == null)
            {
                Clear();
                return;
            }

            Title = SelectedBook.Title;
            Description = SelectedBook.Description;
            Pages = SelectedBook.Pages;
            PublicationYear = SelectedBook.PublicationYear;
            Quantity = SelectedBook.Quantity;
            CostPrice = SelectedBook.CostPrice;
            SalePrice = SelectedBook.SalePrice;
            Genre = SelectedBook.Genre;
            AuthorName = SelectedBook.AuthorName;
            AuthorSurname = SelectedBook.AuthorSurname;
            AuthorPatronymic = SelectedBook.AuthorPatronymic;
            Publisher = SelectedBook.Publisher;
            Series = SelectedBook.Series;
        }

        private void SetProperty<T>(ref T oldValue, T newValue, string propertyName)
        {
            if (!oldValue?.Equals(newValue) ?? newValue != null)
            {
                oldValue = newValue;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }




    // Commands section
    internal partial class BookstoreViewModel
    {
        private readonly AutoEventCommandBase _searchCommand;
        private readonly AutoEventCommandBase _resetCommand;
        private readonly AutoEventCommandBase _saveCommand;
        private readonly AutoEventCommandBase _createCommand;
        private readonly AutoEventCommandBase _cancelCommand;
        private readonly AutoEventCommandBase _clearCommand;
        private readonly AutoEventCommandBase _deleteCommand;

        public AutoEventCommandBase SearchCommand => _searchCommand;
        public AutoEventCommandBase ResetCommand => _resetCommand;
        public AutoEventCommandBase SaveCommand => _saveCommand;
        public AutoEventCommandBase CreateCommand => _createCommand;
        public AutoEventCommandBase CancelCommand => _cancelCommand;
        public AutoEventCommandBase ClearCommand => _clearCommand;
        public AutoEventCommandBase DeleteCommand => _deleteCommand;

        private void Search()
        {
            BookViewModel[] selection;
            switch (SelectedItem)
            {
                case "Title":
                    selection = Books.Where(b => b.Title.Contains(SearchTextBox)).ToArray();
                    break;

                case "Author":
                    selection = Books.Where(b =>
                    b.AuthorName.Contains(SearchTextBox)
                    || b.AuthorSurname.Contains(SearchTextBox)
                    || b.AuthorPatronymic.Contains(SearchTextBox)).ToArray();
                    break;

                case "Genre":
                    selection = Books.Where(b => b.Genre.Contains(SearchTextBox)).ToArray();
                    break;

                default:
                    return;
            }

            Reset();
        }

        private void Reset()
        {
            Books.Clear();
            Books = new List<BookViewModel>(_model.GetBooks());
            SelectedBook = null;
        }

        private void Save()
        {
            _model.UpdateBook(new BookViewModel(SelectedBook.Id)
            {
                Title = SelectedBook.Title,
                Description = SelectedBook.Description,
                Pages = SelectedBook.Pages,
                PublicationYear = SelectedBook.PublicationYear,
                Quantity = SelectedBook.Quantity,
                CostPrice = SelectedBook.CostPrice,
                SalePrice = SelectedBook.SalePrice,
                Genre = SelectedBook.Genre,
                AuthorName = SelectedBook.AuthorName,
                AuthorSurname = SelectedBook.AuthorSurname,
                Publisher = SelectedBook.Publisher,
                Series = SelectedBook.Series
            });
            Reset();
        }

        private bool CanSave() => WereFieldChanged && !IsRequiredFieldsEmpty && IsNumericFieldsCorrect;

        private void Create()
        {
            if (SelectedBook != null)
            {
                _model.AddBook(SelectedBook);
            }
            else
            {
                _model.AddBook(new BookViewModel(-1)
                {
                    Title = this.Title,
                    Description = this.Description,
                    Pages = this.Pages,
                    PublicationYear = this.PublicationYear,
                    Quantity = this.Quantity,
                    CostPrice = this.CostPrice,
                    SalePrice = this.SalePrice,
                    Genre = this.Genre,
                    AuthorName = this.AuthorName,
                    AuthorSurname = this.AuthorSurname,
                    Publisher = this.Publisher,
                    Series = this.Series
                });
            }

            Reset();
        }

        private bool CanCreate() => !IsRequiredFieldsEmpty && IsNumericFieldsCorrect;

        private void Cancel()
        {
            OnSelectionChange(this, new PropertyChangedEventArgs(nameof(SelectedBook)));
        }

        private bool CanCancel() => WereFieldChanged;

        private void Clear()
        {
            Title = "";
            Description = "";
            Pages = "";
            PublicationYear = "";
            Quantity = "";
            CostPrice = "";
            SalePrice = "";
            Genre = "";
            AuthorName = "";
            AuthorSurname = "";
            AuthorPatronymic = "";
            Publisher = "";
            Series = "";
        }

        private bool CanClear() => !IsRequiredFieldsEmpty;

        private void Delete()
        {
            _model.DeleteBook(SelectedBook.Id);
            Reset();
        }

        private bool CanDelete() => IsBookSelected;
    }
}
