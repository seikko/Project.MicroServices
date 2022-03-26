using Course.Shared.Dtos;
using Course.Shared.Services;
using Course.Web.Models.Orders;
using Course.Web.Models.Payments;
using Course.Web.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Concretes
{
    public class OrderServices : IOrderServices
    {
        private readonly HttpClient _client;
        private readonly IBasketServices _basketServices;
        private readonly IPaymentServices _paymentServices;
        private readonly ISharedIdentityServices _sharedIdentityServices;

        public OrderServices(HttpClient client, IBasketServices basketServices, IPaymentServices paymentServices, ISharedIdentityServices sharedIdentityServices)
        {
            _client = client;
            _basketServices = basketServices;
            _paymentServices = paymentServices;
            _sharedIdentityServices = sharedIdentityServices;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _client.GetFromJsonAsync<Response<List<OrderViewModel>>>("getOrders");
            if (!response.IsSuccessfull) return null;
            return response.Data;


        }

        public async Task<OrderCreatedViewModel> OrderCreate(CheckoutInfoModel checkoutInfoModel)
        {
            var basket = await _basketServices.GetBasket();
            var payment = new PaymentModel
            {
                CardName = checkoutInfoModel.CardName,
                CardNumber = checkoutInfoModel.CardNumber,
                CVV = checkoutInfoModel.CVV,
                Expression = checkoutInfoModel.Expression,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment = await _paymentServices.ReceivePayment(payment);
            if (!responsePayment) return new OrderCreatedViewModel() { Errors = "Odeme Alınamadı", IsSuccesfull = false };

            var orderCreate = new CreateOrderInputModel
            {
                BuyerId = _sharedIdentityServices.GetUserId,
                Address = new AddressCreateInput
                {
                    Province = checkoutInfoModel.Province ?? "",
                    District = checkoutInfoModel.District ?? "",
                    AddresLine = checkoutInfoModel.Line ?? "",
                    Street = checkoutInfoModel.Street ?? "",
                    ZipCode = checkoutInfoModel.ZipCode ?? "",

                },
            };
            basket.BasketItem.ForEach(x =>
            {
                var orderItem = new OrderItemViewModel
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    PictureUrl = x.PictureUrl,
                    ProductName = x.CourseName

                };
                orderCreate.OrderItem.Add(orderItem);
            });

            var response = await _client.PostAsJsonAsync<CreateOrderInputModel>("orders", orderCreate);
            if (!response.IsSuccessStatusCode) return new OrderCreatedViewModel() { Errors = "Sipariş Oluşturulamadı", IsSuccesfull = false };
            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync <Response<OrderCreatedViewModel>>();

            orderCreatedViewModel.Data.IsSuccesfull = true;
            await _basketServices.Delete();
            return orderCreatedViewModel.Data;

        }

        public Task SuspendOrder(CheckoutInfoModel checkoutInfo)
        {
            throw new NotImplementedException();
        }
    }
}
