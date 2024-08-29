﻿using BookShopping.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopping.Controllers
{
    [Authorize(Roles=nameof(Roles.Admin))]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepo;


        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        public async Task<IActionResult> Index(string sTerm="")
        {
            var stocks = await _stockRepo.GetStocks(sTerm);
            return View(stocks);
        }



        public async Task<IActionResult> ManageStock(int bookId)
        {
            var existingStock = await _stockRepo.GetStockByBookId(bookId);
            var stock = new StockDTO
            {
                BookId = bookId,
                Quantity = existingStock != null ? existingStock.Quantity : 0
            };
            return View(stock);
           
        }

        [HttpPost]
        public async Task<IActionResult> ManageStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
                return View(stock);
            
            try
                {
                    await _stockRepo.ManageStock(stock);
                    TempData["SuccessMessage"] = "Stock is updated successfully";
             }catch(Exception ex)
                {
                    TempData["SuccessMessage"] = "Stock went wrong";
                }


            return RedirectToAction(nameof(Index));
                
        } 








    }
}
