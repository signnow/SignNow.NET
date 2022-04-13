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

    internal class Pagination
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
    }
}
