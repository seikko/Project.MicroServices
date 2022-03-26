using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Orders
{
    public class OrderCreatedViewModel
    {
        public int OrderId { get; set; }
        public string Errors { get; set; }
        public bool IsSuccesfull { get; set; }

    }
}
