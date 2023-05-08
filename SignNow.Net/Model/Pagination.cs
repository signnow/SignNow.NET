using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    public class Pagination
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

        [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
        public PageLinks Links { get; set; }
    }

    public class PageLinks
    {
        [JsonProperty("previous")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Previous { get; set; }

        [JsonProperty("next")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Next { get; set; }
    }
}
