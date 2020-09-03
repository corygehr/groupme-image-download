using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoryGehr.GroupMe.GetImages.Models
{
    /// <summary>
    /// Group API Response class
    /// </summary>
    [JsonObject]
    public class Group
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public long CreatorUserId { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
        public List<GroupMember> Members { get; set; }
        public string ShareUrl { get; set; }
        public GroupMessagesOverview Messages { get; set; }
    }
}
