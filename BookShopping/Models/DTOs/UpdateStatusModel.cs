﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookShopping.Models.DTOs
{
    public class UpdateStatusModel
    {
        public int OrderId { get; set; }
        [Required]
        public int OrderStatusId { get; set; }
        public IEnumerable<SelectListItem>? OrderStatusList { get; set; }
    }
}
