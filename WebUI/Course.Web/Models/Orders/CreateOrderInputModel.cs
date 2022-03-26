using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Orders
{
    public class CreateOrderInputModel
    {
        public CreateOrderInputModel()
        {
            OrderItem = new List<OrderItemViewModel>();
        }
        public string BuyerId { get; set; }
        public List<OrderItemViewModel>OrderItem { get; set; }
        public AddressCreateInput Address { get; set; }
    }
}
