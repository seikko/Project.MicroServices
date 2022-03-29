using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Shared.Messages
{
    public class CourseNameChanceEvent
    {
        public string CourseId { get; set; }
        public string UpdatedName { get; set; }
        
    }
}
