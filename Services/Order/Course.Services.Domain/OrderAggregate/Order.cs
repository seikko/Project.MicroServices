using Course.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {//hersey order sınıfından ılerlıcek
        public DateTime OrderDate { get; private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private set; }//userıd
        private readonly List<OrderItem> _orderItems; //propertyden almak yerine field uzerınden alıyor
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;//sadece okuma 

        public Order(string buyerId, Address address)
        {
            _orderItems = new List<OrderItem>();
            {
                OrderDate = DateTime.Now;
                BuyerId = buyerId;
                Address = address;
            }
        }
        public Order()
        {

        }
        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);
            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItems.Add(newOrderItem);

            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);

    }
}
