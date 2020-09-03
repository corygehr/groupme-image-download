using System;
using System.Collections.Generic;
using System.Text;

namespace CoryGehr.GroupMe.GetImages.Models
{
    public class GroupMessage
    {
        public string Nickname { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
