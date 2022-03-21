using Course.Web.Exceptions;
using Course.Web.Models;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogServices _catalogServices;
        public HomeController(ILogger<HomeController> logger,ICatalogServices catalogServices)
        {
            _logger = logger;
            _catalogServices = catalogServices;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _catalogServices.GetAllCoursesAsync();
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {   
            return View(await _catalogServices.GetByCourseIdAsync(id));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if(errorFeature != null && errorFeature.Error is UnAuthroizeException)
            {
                return RedirectToAction(nameof(AuthController.LogOut), "Auth");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
