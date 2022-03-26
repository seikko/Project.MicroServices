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
    public class PaymentServices : IPaymentServices
    {
        private readonly HttpClient _client;

        public PaymentServices(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> ReceivePayment(PaymentModel payment)
        {
            var response = await _client.PostAsJsonAsync<PaymentModel>("fakepayment", payment);
             
            return response.IsSuccessStatusCode;
        }
    }
}
