using System;
using System.Collections.Generic;
using System.Text;

namespace CoryGehr.GroupMe.GetImages.Models
{
    public class GroupMember
    {
        public long UserId { get; set; }
        public string Nickname { get; set; }
        public bool Muted { get; set; }
        public string ImageUrl { get; set; }
    }
}
