using Course.Services.Discount.Services;
using Course.Shared.ControllerBases;
using Course.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomeBasController
    {
        private readonly IDiscountService _services;
        private readonly ISharedIdentityServices _sharedIdentityServices;

        public DiscountsController(IDiscountService services, ISharedIdentityServices sharedIdentityServices)
        {
            _services = services;
            _sharedIdentityServices = sharedIdentityServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _services.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            return CreateActionResultInstance(await _services.GetById(id));
        }

        [HttpGet()]
        [Route("/api/[controller]/[action]/{code}")]
         public async Task<IActionResult> GetByDiscountCode(string code)
        {
            var userId = _sharedIdentityServices.GetUserId;
            return CreateActionResultInstance(await _services.GetByCodeAndUserId(code, userId));
        }
        
        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount)
        {
            return CreateActionResultInstance(await _services.Save(discount));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount)
        {
            return CreateActionResultInstance(await _services.Update(discount));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _services.Delete(id));
        }



    }
}
