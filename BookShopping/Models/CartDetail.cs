using System.ComponentModel.DataAnnotations;

namespace BookShopping.Models
{
    public class CartDetail
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartid { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public Book Book { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
