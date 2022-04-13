using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    public class EventSubscription
    {
        /// <summary>
        /// Unique identifier of Event.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("entity_unique_id")]
        public string EntityUid { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("json_attributes")]
        public EventAttributes JsonAttributes { get; set; }

        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Timestamp document was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }
    }

    public class EventAttributes
    {
        [JsonProperty("use_tls_12")]
        public bool UseTls12 { get; set; }

        [JsonProperty("docid_queryparam")]
        public bool DocIdQueryParam { get; set; }

        [JsonProperty("callback_url")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri CallbackUrl { get; set; }
    }
}
