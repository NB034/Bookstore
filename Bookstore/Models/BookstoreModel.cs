using Bookstore.ViewModels;
using System;
using Bookstore.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Bookstore.DataAccess.Entities;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task AddBook(BookViewModel book)
        {
            await _dbContext.BookEntities.AddAsync(new BookEntity
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
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBook(int id)
        {
            var book = await _dbContext.BookEntities.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (book == null) throw new ArgumentException($"The item with id {id} is not exist");
            var ids = new int[] { book.AuthorId, book.PublisherId, book.GenreId, book.SeriesId };
            _dbContext.BookEntities.Remove(book);
            _dbContext.SaveChanges();

            await DeleteAuthorIfNotUsed(ids[0]);
            await DeletePublisherIfNotUsed(ids[1]);
            await DeleteGenreIfNotUsed(ids[2]);
            await DeleteSeriesIfNotUsed(ids[3]);
        }

        public async Task<BookViewModel[]> GetBooks()
        {
            var books = await _dbContext.BookEntities.ToArrayAsync();
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

        public async Task UpdateBook(BookViewModel bookVm)
        {
            var book = await _dbContext.BookEntities.Where(b => b.Id == bookVm.Id).FirstOrDefaultAsync();
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

            _dbContext.Update(book);
            await _dbContext.SaveChangesAsync();
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

        private async Task DeleteSeriesIfNotUsed(int seriesId)
        {
            if (await _dbContext.BookEntities.FirstOrDefaultAsync(b => b.SeriesId == seriesId) != null) return;
            _dbContext.SeriesEntities.Remove(await _dbContext.SeriesEntities.Where(s => s.Id == seriesId).FirstAsync());
            await _dbContext.SaveChangesAsync();
        }

        private async Task DeletePublisherIfNotUsed(int publisherId)
        {
            if (await _dbContext.BookEntities.FirstOrDefaultAsync(b => b.PublisherId == publisherId) != null) return;
            _dbContext.PublisherEntities.Remove(await _dbContext.PublisherEntities.Where(p => p.Id == publisherId).FirstAsync());
            await _dbContext.SaveChangesAsync();
        }

        private async Task DeleteGenreIfNotUsed(int genreId)
        {
            if (await _dbContext.BookEntities.FirstOrDefaultAsync(b => b.GenreId == genreId) != null) return;
            _dbContext.GenreEntities.Remove(await _dbContext.GenreEntities.Where(g => g.Id == genreId).FirstAsync());
            await _dbContext.SaveChangesAsync();
        }

        private async Task DeleteAuthorIfNotUsed(int authorId)
        {
            if (await _dbContext.BookEntities.FirstOrDefaultAsync(b => b.AuthorId == authorId) != null) return;
            _dbContext.AuthorEntities.Remove(await _dbContext.AuthorEntities.Where(a => a.Id == authorId).FirstAsync());
            await _dbContext.SaveChangesAsync();
        }
    }
}
