using Course.Web.Models.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Services.Abstract
{
    public interface IDiscountServices
    {
        Task<DiscountModel> GetDiscount(string code);

    }
}
