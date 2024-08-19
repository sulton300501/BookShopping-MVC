namespace BookShopping.Models.DTOs
{
    public class BookDisplayModel
    {
        public IEnumerable<Book> Books { get; set;}
        public IEnumerable<Genre> Genres { get; set;}
    }
}
