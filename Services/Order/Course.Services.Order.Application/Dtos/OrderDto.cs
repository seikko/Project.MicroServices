using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Dtos
{
    public class OrderDto
    {
        public int Id  { get; set; }
        public DateTime OrderDate { get; private set; }
        public AddressDto Address { get; private set; }
        public string BuyerId { get; private set; }//userıd

        public List<OrderItemDto> OrderItem { get; set; }

    }
}
