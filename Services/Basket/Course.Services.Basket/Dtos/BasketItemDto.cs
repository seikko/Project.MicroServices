using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Basket.Dtos
{
    public class BasketItemDto
    {

        public string CourseId { get; set; }
        public int Quantity { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }


    }
}
