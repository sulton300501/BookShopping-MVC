namespace BookShopping.Models.DTOs
{
    public class OrderDetailModalDTO
    {

        public string  Divid {  get; set; }
        public IEnumerable<OrderDetails> OrderDetail {  get; set; }
    }
}
