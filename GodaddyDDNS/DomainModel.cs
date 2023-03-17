using Newtonsoft.Json;
using System;

namespace GodaddyDDNS
{
    public class DomainModel{

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ttl")]
        public int TTL { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

    }

}
