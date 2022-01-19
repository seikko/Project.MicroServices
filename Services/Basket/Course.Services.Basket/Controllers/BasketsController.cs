using Course.Services.Basket.Dtos;
using Course.Services.Basket.Services;
using Course.Shared.ControllerBases;
using Course.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomeBasController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityServices _sharedIdentityServices;

        public BasketsController(IBasketService basketService, ISharedIdentityServices sharedIdentityServices)
        {
            _basketService = basketService;
            _sharedIdentityServices = sharedIdentityServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
           // var claims = User.Claims; ne gelıyor diye bakmak icin

            return CreateActionResultInstance(await _basketService.GetBasket(_sharedIdentityServices.GetUserId));
        }
        
        [HttpPost]
        public async Task<IActionResult>SaveOrUpdateBasket(BasketDto basketDto)
        {
            var response = await _basketService.SaveOrUpdate(basketDto);
            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return CreateActionResultInstance(await _basketService.DeleteAllBasket(_sharedIdentityServices.GetUserId));
        }
    }
}
