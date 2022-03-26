using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Payments
{
    public class PaymentModel
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expression { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
