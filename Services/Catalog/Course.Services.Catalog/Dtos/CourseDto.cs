using Course.Services.Catalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Dtos
{
    public class CourseDto
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }

        public decimal Price { get; set; }
        public string Picture { get; set; }

        public DateTime CreatedDate { get; set; }

        //Relational Properties

        public string CategoryId { get; set; }
        public Feature Feature { get; set; }
        public Category Category { get; set; }
    }
}
