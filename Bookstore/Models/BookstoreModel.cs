using Bookstore.ViewModels;
using System;
using Bookstore.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Bookstore.DataAccess.Entities;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Bookstore.Models
{
    partial class BookstoreModel : IBookstoreModel
    {
        private readonly BookstoreDbContext _dbContext;

        public BookstoreModel()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("Configuration//appsettings.json");

            var config = configBuilder.Build();
            string connectionString = config.GetConnectionString("connectionString");

            _dbContext = ContextBuilder.GetBookstoreDbContext(connectionString);
        }

        public void AddBook(BookViewModel book)
        {
            _dbContext.BookEntities.Add(new BookEntity
            {
                Title = book.Title,
                Description = book.Description,
                Pages = int.Parse(book.Pages),
                PublicationYear = int.Parse(book.PublicationYear),
                Quantity = int.Parse(book.Quantity),
                CostPrice = int.Parse(book.CostPrice),
                SalePrice = int.Parse(book.SalePrice),
                GenreId = CreateGenreIfNotExist(book.Genre),
                AuthorId = CreateAuthorIfNotExist(book.AuthorName, book.AuthorSurname, book.AuthorPatronymic),
                PublisherId = CreatePublisherIfNotExist(book.Publisher),
                SeriesId = CreateSeriesIfNotExist(book.Series)
            });
            _dbContext.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var book = _dbContext.BookEntities.Where(b => b.Id == id).FirstOrDefault();
            if (book == null) throw new ArgumentException($"The item with id {id} is not exist");
            var ids = new int[] { book.AuthorId, book.PublisherId, book.GenreId, book.SeriesId };
            _dbContext.BookEntities.Remove(book);
            _dbContext.SaveChanges();

            DeleteAuthorIfNotUsed(ids[0]);
            DeletePublisherIfNotUsed(ids[1]);
            DeleteGenreIfNotUsed(ids[2]);
            DeleteSeriesIfNotUsed(ids[3]);
        }

        public BookViewModel[] GetBooks()
        {
            var books = _dbContext.BookEntities;
            List<BookViewModel> result = new List<BookViewModel>();

            foreach (var book in books)
            {
                var author = _dbContext.AuthorEntities.Where(a => a.Id == book.AuthorId).FirstOrDefault();

                result.Add(new BookViewModel(book.Id)
                {
                    Title = book.Title,
                    Description = book.Description,
                    Pages = book.Pages.ToString(),
                    PublicationYear = book.PublicationYear.ToString(),
                    Quantity = book.Quantity.ToString(),
                    CostPrice = book.CostPrice.ToString(),
                    SalePrice = book.SalePrice.ToString(),
                    Publisher = _dbContext.PublisherEntities.Where(p => p.Id == book.PublisherId).FirstOrDefault().Name,
                    Genre = _dbContext.GenreEntities.Where(g => g.Id == book.GenreId).FirstOrDefault().Name,
                    Series = _dbContext.SeriesEntities.Where(s => s.Id == book.SeriesId).FirstOrDefault().Name,
                    AuthorName = author.Name,
                    AuthorSurname = author.Surname,
                    AuthorPatronymic = author.Patronymic,
                });
            }

            return result.ToArray();
        }

        public void UpdateBook(BookViewModel bookVm)
        {
            var book = _dbContext.BookEntities.Where(b => b.Id == bookVm.Id).FirstOrDefault();
            if (book == null) throw new ArgumentException($"The item with id {bookVm.Id} is not exist");

            book.Title = bookVm.Title;
            book.Description = bookVm.Description;
            book.Pages = int.Parse(bookVm.Pages);
            book.PublicationYear = int.Parse(bookVm.PublicationYear);
            book.Quantity = int.Parse(bookVm.Quantity);
            book.CostPrice = int.Parse(bookVm.CostPrice);
            book.SalePrice = int.Parse(bookVm.SalePrice);
            book.GenreId = CreateGenreIfNotExist(bookVm.Genre);
            book.AuthorId = CreateAuthorIfNotExist(bookVm.AuthorName, bookVm.AuthorSurname, bookVm.AuthorPatronymic);
            book.PublisherId = CreatePublisherIfNotExist(bookVm.Publisher);
            book.SeriesId = CreateSeriesIfNotExist(bookVm.Series);
            
            _dbContext.SaveChanges();
        }
    }



    //Private members
    partial class BookstoreModel
    {
        private int CreateSeriesIfNotExist(string seriesName)
        {
            var series = _dbContext.SeriesEntities.Where(s => s.Name == seriesName).FirstOrDefault();
            if (series != null) return series.Id;
            var newItem = _dbContext.SeriesEntities.Add(new SeriesEntity { Name = seriesName });
            _dbContext.SaveChanges();
            return newItem.Entity.Id;
        }

        private int CreatePublisherIfNotExist(string publisherName)
        {
            var publisher = _dbContext.PublisherEntities.Where(p => p.Name == publisherName).FirstOrDefault();
            if (publisher != null) return publisher.Id;
            var newItem = _dbContext.PublisherEntities.Add(new PublisherEntity { Name = publisherName });
            _dbContext.SaveChanges();
            return newItem.Entity.Id;
        }

        private int CreateGenreIfNotExist(string genreName)
        {
            var genre = _dbContext.GenreEntities.Where(g => g.Name == genreName).FirstOrDefault();
            if (genre != null) return genre.Id;
            var newItem = _dbContext.GenreEntities.Add(new GenreEntity { Name = genreName });
            _dbContext.SaveChanges();
            return newItem.Entity.Id;
        }

        private int CreateAuthorIfNotExist(string authorName, string authorSurname, string authorPatronymic)
        {
            var author = _dbContext.AuthorEntities.Where(a => a.Name == authorName
                                                            && a.Surname == authorSurname
                                                            && a.Patronymic == authorPatronymic).FirstOrDefault();
            if (author != null) return author.Id;
            var newItem = _dbContext.AuthorEntities.Add(new AuthorEntity
            {
                Name = authorName,
                Surname = authorSurname,
                Patronymic = authorPatronymic
            });
            _dbContext.SaveChanges();
            return newItem.Entity.Id;
        }

        private void DeleteSeriesIfNotUsed(int seriesId)
        {
            if (_dbContext.BookEntities.FirstOrDefault(b => b.SeriesId == seriesId) != null) return;
            _dbContext.SeriesEntities.Remove(_dbContext.SeriesEntities.Where(s => s.Id == seriesId).First());
            _dbContext.SaveChanges();
        }

        private void DeletePublisherIfNotUsed(int publisherId)
        {
            if (_dbContext.BookEntities.FirstOrDefault(b => b.PublisherId == publisherId) != null) return;
            _dbContext.PublisherEntities.Remove(_dbContext.PublisherEntities.Where(p => p.Id == publisherId).First());
            _dbContext.SaveChanges();
        }

        private void DeleteGenreIfNotUsed(int genreId)
        {
            if (_dbContext.BookEntities.FirstOrDefault(b => b.GenreId == genreId) != null) return;
            _dbContext.GenreEntities.Remove(_dbContext.GenreEntities.Where(g => g.Id == genreId).First());
            _dbContext.SaveChanges();
        }

        private void DeleteAuthorIfNotUsed(int authorId)
        {
            if (_dbContext.BookEntities.FirstOrDefault(b => b.AuthorId == authorId) != null) return;
            _dbContext.AuthorEntities.Remove(_dbContext.AuthorEntities.Where(a => a.Id == authorId).First());
            _dbContext.SaveChanges();
        }
    }
}
