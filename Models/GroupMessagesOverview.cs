using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoryGehr.GroupMe.GetImages.Models
{
    [JsonObject]
    public class GroupMessagesOverview
    {
        public int Count { get; set; }
        [JsonProperty("last_message_id")]
        public string LastMessageId { get; set; }
        public long LastMessageCreatedAt { get; set; }
        public GroupMessage Preview { get; set; }
    }
}
