using Course.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Domain.OrderAggregate
{
   public class OrderItem:Entity
    {
        public string ProductId { get; private set; }
        public string ProductName{ get; private set; }
        public string PictureUrl { get; private set; }
        public decimal Price { get;  private set; }
        public OrderItem()
        {

        }
        public OrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }
        public void UpdateOrderItem(string productName,string pictureUrl,decimal price)
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;

        }
    }
}
