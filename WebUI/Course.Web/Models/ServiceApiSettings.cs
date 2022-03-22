using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models
{
    public class ServiceApiSettings
    {
        public string IdentityBaseUrl { get; set; }
        public string GatewayBaseUrl { get; set; }
        public string PhotoStockUri { get; set; }
        public ServiceApi Catalog { get; set; }
        public ServiceApi PhotoStock { get; set; }
        public ServiceApi Basket { get; set; }
        public ServiceApi Discount { get; set; }
    }
    public class ServiceApi
    {
        public string Path { get; set; }
    }
}
