﻿

using Microsoft.EntityFrameworkCore;

namespace BookShopping.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public HomeRepository(ApplicationDbContext context)
        {
            _dbcontext = context;
        }


        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _dbcontext.Genres.ToListAsync();
        }




        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books = await (from book in _dbcontext.Books
                                             join genre in _dbcontext.Genres
                                             on book.GenreId equals genre.Id
                                             join stock in _dbcontext.Stocks
                                             on book.Id equals stock.BookId
                                             into book_stocks
                                             from bookWithStock in book_stocks.DefaultIfEmpty()
                                             where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
                                             select new Book
                                             {
                                                 Id = book.Id,
                                                 Image = book.Image,
                                                 AuthorName = book.AuthorName,
                                                 BookName = book.BookName,
                                                 GenreId = book.GenreId,
                                                 Price = book.Price,
                                                 GenreName = genre.GenreName,
                                                 Quantity = bookWithStock == null ? 0 : bookWithStock.Quantity
                                             }
                         ).ToListAsync();

            if (genreId > 0)
            {
                books = books.Where(b => b.GenreId == genreId);
            }

            return  books;
        }

    }
}
