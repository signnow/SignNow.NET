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

        public string Event { get; set; }
        public int EntityId { get; set; }
        public string EntityUid { get; set; }
        public string Action { get; set; }
        public string JsonAttributes { get; set; }

        /// <summary>
        /// Timestamp document was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }
    }
}
