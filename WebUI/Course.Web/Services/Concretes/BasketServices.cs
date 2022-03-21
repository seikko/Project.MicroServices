using Course.Shared.Dtos;
using Course.Web.Models.Basket;
using Course.Web.Services.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Web.Services.Abstract
{
    public class BasketServices : IBasketServices
    {
        private readonly HttpClient _client;
        public BasketServices(HttpClient client)
        {
            _client = client;
        }

        public async Task AddBasketItem(BasketItemViewModel basketViewModel)
        {
            var basket = await GetBasket();
            if(basket != null)
            {
                if(!basket.BasketItem.Any(x=> x.CourseId == basketViewModel.CourseId))
                {
                    basket.BasketItem.Add(basketViewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.BasketItem.Add(basketViewModel);
                
            }
            await SaveOrUpdate(basket);
        }

        public Task<bool> ApplyDiscount(string discoundCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelApplyDiscount()
        {
            throw new NotImplementedException();
        }

        public async  Task<bool> Delete()
        {


            var response = await _client.DeleteAsync("baskets");
            return response.IsSuccessStatusCode; 
        }

        public async Task<BasketViewModel> GetBasket()
        {
            var response = await _client.GetAsync("baskets");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var res = response.Content.ReadAsStringAsync();
            var basketViewModel = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();

            return basketViewModel.Data;
          
        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {
            var basket = await GetBasket();
            if (basket == null) return false;
           
           
            var findItem = basket.BasketItem.FirstOrDefault(x => x.CourseId == courseId);
            if (findItem == null) return false;
            var deletedItem = basket.BasketItem.Remove(findItem);
            if (!deletedItem) return false;

            if (basket.BasketItem.Any())
            {
                basket.DiscountCode = null;
            }
            return await SaveOrUpdate(basket);


        }

        public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
        {
            var response = await _client.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);
            return response.IsSuccessStatusCode;
        }
    }
}
