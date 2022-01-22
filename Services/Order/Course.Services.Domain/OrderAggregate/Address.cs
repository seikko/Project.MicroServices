using Course.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Domain.OrderAggregate
{
   public class Address:ValueObject
    {
        public string Province { get; private set; } //dısarıdan set edilmesin
        public string District { get; private set; }
        public string  Street { get; private set; }
        public string ZipCode { get; private set; }
        public string AddresLine { get; private set; }

        public Address(string province, string district, string street, string zipCode, string addresLine)
        {
            Province = province;
            District = district;
            Street = street;
            ZipCode = zipCode;
            AddresLine = addresLine;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Province;//property durumları birbirine esşitse equals true
            yield return District;
            yield return Street;
            yield return ZipCode;
            yield return AddresLine;
        }
    }
}
