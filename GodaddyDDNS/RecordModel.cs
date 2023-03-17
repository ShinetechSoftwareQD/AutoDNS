using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GodaddyDDNS
{
    public class RecordModel
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("ttl")]
        public int TTL { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}
