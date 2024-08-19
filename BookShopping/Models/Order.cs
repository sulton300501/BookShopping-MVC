﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopping.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }
        [Required]

        public string UserId { get; set; }
        public DateTime Createddate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } 

        public OrderStatus OrderStatus { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }

    }
}
