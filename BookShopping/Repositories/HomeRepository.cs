

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

            var booksQuery = _dbcontext.Books
                .Join(_dbcontext.Genres,
                    book => book.GenreId,
                    genre => genre.Id,
                    (book, genre) => new Book
                    {
                        Id = book.Id,
                        Image = book.Image,
                        AuthorName = book.AuthorName,
                        BookName = book.BookName,
                        GenreId = book.GenreId,
                        Price = book.Price,
                        GenreName = genre.GenreName,
                    });

            if (!string.IsNullOrWhiteSpace(sTerm))
            {
                booksQuery = booksQuery.Where(b => b.BookName.ToLower().StartsWith(sTerm));
            }

            if (genreId > 0)
            {
                booksQuery = booksQuery.Where(b => b.GenreId == genreId);
            }

            return await booksQuery.ToListAsync();
        }

    }
}
