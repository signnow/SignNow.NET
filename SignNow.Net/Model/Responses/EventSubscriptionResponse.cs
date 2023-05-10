using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class EventSubscriptionResponse
    {
        [JsonProperty("data")]
        public List<EventSubscription> Data { get; internal set; }

        [JsonProperty("meta")]
        public MetaInfo Meta { get; internal set; }
    }

    public class MetaInfo
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; internal set; }
    }
}
