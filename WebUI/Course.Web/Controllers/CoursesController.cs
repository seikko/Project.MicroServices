using Course.Shared.Services;
using Course.Web.Models;
using Course.Web.Models.Catalogs;
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

           var result = await _catalogServices.GetAllCourseByUserIdAsync(_sharedIdentityServices.GetUserId);
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogServices.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
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

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogServices.GetByCourseIdAsync(id);
            var categories = await _catalogServices.GetAllCategoryAsync();
            if (course == null)
            {
                RedirectToAction(nameof(Index));
            }
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.Id);

            CourseUpdateModel courseUpdateModel = new()
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                Feature = course.Feature,
                CategoryId = course.CategoryId,
                UserId = course.UserId,
                Picture = course.Picture,
            };
            return View(courseUpdateModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateModel model)
        {
            var categories = await _catalogServices.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", model.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _catalogServices.UpdateCourseAsync(model);

            return RedirectToAction(nameof(Index));
        }
    
        public async Task<IActionResult> Delete(string id)
        {
            await _catalogServices.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
