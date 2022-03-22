using Course.Web.Models.Basket;
using Course.Web.Models.Discount;
using Course.Web.Services.Concretes;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public class BasketController : Controller
    {
        private readonly ICatalogServices _catalogServices;
        private readonly IBasketServices _basketServices;

        public BasketController(ICatalogServices catalogServices, IBasketServices basketServices)
        {
            _catalogServices = catalogServices;
            _basketServices = basketServices;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketServices.GetBasket());
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogServices.GetByCourseIdAsync(courseId);
            var basketItem = new BasketItemViewModel
            {
                CourseId = course.Id,
                CourseName = course.Name,
                Price = course.Price,
                 
            };
            await _basketServices.AddBasketItem(basketItem);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            var result = await _basketServices.RemoveBasketItem(courseId);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ApplyDiscount(DiscountApplyModel applyModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));

            }
            var discountStatus = await _basketServices.ApplyDiscount(applyModel.Code);
            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> CancelApplyDiscount()
        {
            var discountStatus = await _basketServices.CancelApplyDiscount();
         
            return RedirectToAction(nameof(Index));
        }
    }
}
