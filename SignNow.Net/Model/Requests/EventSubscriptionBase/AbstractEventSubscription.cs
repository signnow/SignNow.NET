using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests.EventSubscriptionBase
{
    public abstract class AbstractEventSubscription : JsonHttpContent
    {
        /// <summary>
        /// Event type.
        /// </summary>
        [JsonProperty("event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType Event { get; set; }

        /// <summary>
        /// The unique ID of the event: "document_id", "user_id", "document_group_id", "template_id".
        /// </summary>
        [JsonProperty("entity_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EntityId { get; protected set; }

        /// <summary>
        /// Always only "callback"
        /// </summary>
        [JsonProperty("action")]
        public string Action = "callback";

        /// <summary>
        /// Event attributes.
        /// </summary>
        [JsonProperty("attributes")]
        public EventCreateAttributes Attributes { get; set; } = new EventCreateAttributes();

        /// <summary>
        /// Enables the HMAC security logic.
        /// </summary>
        [JsonProperty("secret_key", NullValueHandling = NullValueHandling.Ignore)]
        public string SecretKey { get; set; }
    }
}
