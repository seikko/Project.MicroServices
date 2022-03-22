using Course.Shared.Dtos;
using Course.Web.Models.Discount;
using Course.Web.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Concretes
{
    public class DiscountServices : IDiscountServices
    {
        private readonly HttpClient _client;

        public DiscountServices(HttpClient client)
        {
            _client = client;
        }

        public async  Task<DiscountModel> GetDiscount(string code)
        {///api/[controller]/[action]/{code}
            var response = await _client.GetAsync($"discounts/GetByDiscountCode/{code}");
            if (!response.IsSuccessStatusCode) return null;
            var discount = await response.Content.ReadFromJsonAsync<Response<DiscountModel>>();
            return discount.Data ?? new DiscountModel();
        }
    }
}
