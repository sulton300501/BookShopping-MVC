﻿using BookShopping.Constants;
using BookShopping.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShopping.Controllers
{
    [Authorize(Roles =nameof(Roles.Admin))]
    public class AdminOperationsController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepository;

        public AdminOperationsController(IUserOrderRepository userOrderRepository)
        {
            _userOrderRepository = userOrderRepository;
        }

      
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepository.UserOrders(true);
            return View(orders);

        }


        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _userOrderRepository.TogglePaymentStatus(orderId);
            }catch(Exception ex)
            {

            }

            return RedirectToAction(nameof(AllOrders));
        }



        public async Task<IActionResult> UpdatePaymentStatus(int orderId)
        {
            var order = await _userOrderRepository.GetOrderById(orderId);
            if(order == null)
            {
                throw new InvalidOperationException($"Order with id:{orderId} does not found");
            }

            var orderStatusList = (await _userOrderRepository.GetOrderStatuses())
                           .Select(orderStatus =>
                           {
                               return new SelectListItem
                               {
                                   Value = orderStatus.Id.ToString(),
                                   Text = orderStatus.StatusName,
                                   Selected = order.OrderStatusId == orderStatus.Id
                               };
                           });

            var data = new UpdateStatusModel
            {
                OrderId = orderId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };

            return View(data);
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePaymentStatus(UpdateStatusModel data)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    data.OrderStatusList = (await _userOrderRepository.GetOrderStatuses())
                           .Select(orderStatus =>
                           {
                               return new SelectListItem
                               {
                                   Value = orderStatus.Id.ToString(),
                                   Text = orderStatus.StatusName,
                                   Selected = data.OrderStatusId == orderStatus.Id
                               };
                           });

                    return View(data);

                }
                await _userOrderRepository.ChangeOrderStatus(data);
                TempData["msg"] = "Updated Successfully";
            }catch(Exception ex)
            {
                TempData["msg"] = "Something went wrong";
            }

            return RedirectToAction(nameof(UpdatePaymentStatus) , new
            { orderId = data.OrderId});

        }



    }
}
