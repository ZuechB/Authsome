using Newtonsoft.Json;
using System.Collections.Generic;

namespace Amplitude.Models
{
    public class RootEvent
    {
        [JsonProperty("event")]
        public List<Event> Event { get; set; }
    }

    public class Event
    {
        public string user_id { get; set; }
        public string event_type { get; set; }
        public Dictionary<string, string> user_properties { get; set; }
        public string country { get; set; }
        public string ip { get; set; }
        public long time { get; set; }
        public Platform platform { get; set; }
        public string os_name { get; set; }
        public string os_version { get; set; }
        public string device_brand { get; set; }
        public string device_manufacturer { get; set; }
    }

    public enum Platform
    {
        iOS,
        Android,
        Web
    }
}