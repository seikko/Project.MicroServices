using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Models.Catalogs
{
    public class CourseUpdateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public IFormFile PhotoFormFile { get; set; }



        public FeatureViewModel Feature { get; set; }
        //public CategoryViewModel Categories { get; set; }
    }
}
