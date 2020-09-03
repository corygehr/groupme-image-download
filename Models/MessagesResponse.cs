using System;
using System.Collections.Generic;
using System.Text;

namespace CoryGehr.GroupMe.GetImages.Models
{
    public class MessagesResponse
    {
        public long Count { get; set; }
        public List<Message> Messages { get; set; }
    }
}
