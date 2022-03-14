using Course.Shared.Services;
using Course.Web.Models;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    [Authorize]

    public class CoursesController : Controller
    {
        private readonly ICatalogServices _catalogServices;
        private readonly ISharedIdentityServices _sharedIdentityServices;

        public CoursesController(ICatalogServices catalogServices, ISharedIdentityServices sharedIdentityServices)
        {
            _catalogServices = catalogServices;
            _sharedIdentityServices = sharedIdentityServices;
        }

        public async Task<IActionResult> Index()
        {
             
            return View(await _catalogServices.GetAllCourseByUserIdAsync(_sharedIdentityServices.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogServices.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories,"Id","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CourseCreate(CourseCreateModel model)
        {
            var categories = await _catalogServices.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid) return View();

            model.UserId = _sharedIdentityServices.GetUserId;
            await _catalogServices.CreateCourseAsync(model);
            return RedirectToAction(nameof(Index));

        }
    }
}
