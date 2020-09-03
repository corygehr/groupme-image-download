using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoryGehr.GroupMe.GetImages.Models
{
    [JsonObject]
    public class Attachment
    {
        public List<int[]> CharMap { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Name { get; set; }
        public string Placeholder { get; set; }
        public string Token { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
