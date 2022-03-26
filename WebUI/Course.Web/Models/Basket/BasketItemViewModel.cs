using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Basket
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; } 

        public string CourseId { get; set; }
        public string CourseName { get; set; }

        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        private decimal? DiscountAppliedPrice;

        public decimal GetCurrentPrice
        {
            get => DiscountAppliedPrice != null ? DiscountAppliedPrice.Value : Price;
        }

        public void AppliedDiscount(decimal discountPrice)
        {
            DiscountAppliedPrice = discountPrice;
        }

    }
}
