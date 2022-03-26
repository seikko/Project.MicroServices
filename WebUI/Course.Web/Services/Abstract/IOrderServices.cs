using Course.Web.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Services.Abstract
{
    public interface IOrderServices
    {
        /// <summary>
        /// Senkron Iletişim-direk microservice istegi atıcak.
        /// </summary>
        /// <param name="orderCreatedViewModel"></param>
        /// <returns></returns>
        Task<OrderCreatedViewModel> OrderCreate(CheckoutInfoModel orderCreatedViewModel);
        /// <summary>
        /// Async Iletişim-Sipariş Bilgileri Rabbit MQ'ya Gonderilen
        /// </summary>
        /// <param name="checkoutInfo"></param>
        /// <returns></returns>
        Task SuspendOrder(CheckoutInfoModel checkoutInfo);

        /// <summary>
        /// Tüm siparişleri Al
        /// </summary>
        /// <returns></returns>
        Task<List<OrderViewModel>> GetOrder();
    }
}
