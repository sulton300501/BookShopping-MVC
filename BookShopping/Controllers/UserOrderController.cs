﻿using Microsoft.AspNetCore.Mvc;

namespace BookShopping.Controllers
{
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepo;

        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }

        public async Task<IActionResult> UserOrders()
        {
            var orders = await _userOrderRepo.UserOrders();
            return View();
        }
    }
}
