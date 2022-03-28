using Course.Web.Models.Orders;
using Course.Web.Services.Abstract;
using Course.Web.Services.Concretes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketServices _basketServices;
        private readonly IOrderServices _orderServices;

        public OrderController(IBasketServices basketServices, IOrderServices orderServices)
        {
            _basketServices = basketServices;
            _orderServices = orderServices;
        }

        public async  Task<IActionResult> CheckOut()
        {

            var basket = await _basketServices.GetBasket();
            ViewBag.basket = basket;
            return View(new CheckoutInfoModel());
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckoutInfoModel checkoutInfoModel)
        {
            var orderStatus = await _orderServices.SuspendOrder(checkoutInfoModel);
         //   var orderStatus = await _orderServices.OrderCreate(checkoutInfoModel); senkron iletişim rabbitmq olmadan
            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketServices.GetBasket();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();
            }
            //return RedirectToAction(nameof(SuccessfulCheckout),new {orderId = orderStatus.OrderId});
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1, 10000) });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
