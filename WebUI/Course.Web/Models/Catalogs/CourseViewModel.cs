using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models
{
    public class CourseViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string ShortDescription { get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description;}

        public decimal Price { get; set; }
        public string Picture { get; set; }

        public DateTime CreatedDate { get; set; }

        //Relational Properties

        public string CategoryId { get; set; }
        public FeatureViewModel Feature { get; set; }
        public CategoryViewModel Categories { get; set; }
    }
}
