using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Dtos
{
  public  class OrderItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }
}
