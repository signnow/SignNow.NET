using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class EventSubscriptionResponse
    {
        [JsonProperty("data")]
        public IReadOnlyList<EventSubscription> Data { get; internal set; }

        [JsonProperty("meta")]
        public MetaInfo Meta { get; internal set; }
    }
}
