using Course.Web.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Services.Concretes
{
    public interface IBasketServices
    {
        Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);
        Task<BasketViewModel> GetBasket();
        Task<bool> Delete();
        Task AddBasketItem(BasketItemViewModel basketViewModel);
        Task<bool> RemoveBasketItem(string courseId);
        Task<bool> ApplyDiscount(string discoundCode);

        Task<bool> CancelApplyDiscount();
    }
}
