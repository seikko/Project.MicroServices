using Course.Services.Order.Application.Commands;
using Course.Services.Order.Application.Queries;
using Course.Shared.ControllerBases;
using Course.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Order.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomeBasController
    {
        private readonly IMediator _mediater;
        private readonly ISharedIdentityServices _identityService;

        public OrdersController(IMediator mediater, ISharedIdentityServices identityService)
        {
            _mediater = mediater;
            _identityService = identityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {

            var response = await _mediater.Send(new GetOrderByUserIdQuery { UserId = _identityService.GetUserId });
            return CreateActionResultInstance(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(CreateOrderCommand command)
        {

            command.BuyyerId = _identityService.GetUserId;
            var response = await _mediater.Send(command);
            return CreateActionResultInstance(response);
        }
    }
}
