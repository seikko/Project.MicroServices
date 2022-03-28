using Course.Services.FakePayment.Models;
using Course.Shared.ControllerBases;
using Course.Shared.Dtos;
using Course.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.FakePayment.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomeBasController
    {
        private readonly ISendEndpointProvider _sendEndPointProvider;

        public FakePaymentController(ISendEndpointProvider sendEndPointProvider)
        {
            _sendEndPointProvider = sendEndPointProvider;
        }

        [HttpPost()]
        public async Task<IActionResult> ReceivePayment(PaymentDto payment)
        {
            var sendEndpoint = await _sendEndPointProvider.GetSendEndpoint(new Uri("queue:order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand();

            createOrderMessageCommand.BuyerId = payment.Order.BuyerId;
            createOrderMessageCommand.District = payment.Order.Address.District??"";
            createOrderMessageCommand.Line = payment.Order.Address.Line ?? "";
            createOrderMessageCommand.Province = payment.Order.Address.Province??"";
            createOrderMessageCommand.Street = payment.Order.Address.Street??"";
            createOrderMessageCommand.ZipCode = payment.Order.Address.ZipCode??"";


            payment.Order.OrderItem.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl = x.PictureUrl,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName
                });
            });

            try
            {
                await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            }
            catch (Exception ex)
            {

                throw;
            }

            return CreateActionResultInstance(Shared.Dtos.Response<NoContent>.Success(200));
        }
    }
}
