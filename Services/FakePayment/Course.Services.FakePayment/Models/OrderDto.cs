using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.FakePayment.Models
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderItem = new List<OrderItemDto>();
        }
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItem { get; set; }
        public AddressDto Address { get; set; }
    } 
}
