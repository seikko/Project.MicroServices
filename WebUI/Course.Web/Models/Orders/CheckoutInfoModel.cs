using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Orders
{
    public class CheckoutInfoModel
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expression { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
