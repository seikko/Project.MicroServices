using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Dtos
{
   public class AddressDto
    {
        public string Province { get;  set; } //dısarıdan set edilmesin
        public string District { get;  set; }
        public string Street { get;  set; }
        public string ZipCode { get;  set; }
        public string AddresLine { get;  set; }
    }
}
