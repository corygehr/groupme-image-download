using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoryGehr.GroupMe.GetImages.Models
{
    [JsonObject]
    public class Message
    {
        public string Id { get; set; }
        public string SourceGuid { get; set; }
        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Text { get; set; }
        public bool System { get; set; }
        public List<int> FavoritedBy { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
