using System;
using System.Collections.Generic;
using System.Text;

namespace CoryGehr.GroupMe.GetImages.Models
{
    public class ApiResponse<T>
    {
        public int Count { get; set; }
        public T Response { get; set; }
        public ApiResponseMetadata Meta { get; set; }
    }
}
