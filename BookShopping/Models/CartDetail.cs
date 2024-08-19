using System.ComponentModel.DataAnnotations;

namespace BookShopping.Models
{
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCart_id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public Book Book { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
