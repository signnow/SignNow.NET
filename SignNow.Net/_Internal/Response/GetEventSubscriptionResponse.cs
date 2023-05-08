using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model;

namespace SignNow.Net._Internal.Response
{
    internal class GetEventSubscriptionResponse
    {
        [JsonProperty("data")]
        public List<EventSubscription> Data { get; internal set; }

        [JsonProperty("meta")]
        public MetaInfo Meta { get; internal set; }
    }

    internal class MetaInfo
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; internal set; }
    }
}
