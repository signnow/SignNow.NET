using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents a webhook callback event in the SignNow system.
    /// </summary>
    public class Callback
    {
        /// <summary>
        /// Unique identifier of the callback.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        // application_name string

        /// <summary>
        /// The entity ID associated with the callback.
        /// </summary>
        [JsonProperty("entity_id")]
        public string EntityId { get; set; }

        // event_subscription_id uuid

        // event_subscription_active bool

        // entity_type string | enum => seem it is EventSubscriptionEntityType

        [JsonProperty("event_name")]
        public EventType EventName { get; set; }

        /// <summary>
        /// The callback URL that was triggered.
        /// </summary>
        [JsonProperty("callback_url")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri CallbackUrl { get; set; }

        // request_method string | enum ?

        // duration double | decimal

        /// <summary>
        /// The timestamp when the callback started (Unix timestamp).
        /// </summary>
        [JsonProperty("request_start_time")]
        public long StartTime { get; set; }

        /// <summary>
        /// The timestamp when the callback ended (Unix timestamp).
        /// </summary>
        [JsonProperty("request_end_time")]
        public long EndTime { get; set; }

        [JsonProperty("request_headers", NullValueHandling = NullValueHandling.Ignore)]
        public EventAttributeHeaders Headers { get; set; }

        // request_content > meta & content
        [JsonProperty("request_content")]
        public EventRequestContent RequestContent { get; set; }

        // response_content string

        // response_status_code int

        /// <summary>
        /// Email address of the owner of the event subscription.
        /// </summary>
        [JsonProperty("event_subscription_owner_email")]
        public string EventSubscriptionOwnerEmail { get; set; }
    }

    public class EventRequestContent
    {
        [JsonProperty("meta")]
        public MetaData Meta { get; set; }

        [JsonProperty("content")]
        public ContentData Content { get; set; }

        public class MetaData
        {
            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }
        }

        public class ContentData
        {
            [JsonProperty("document_id")]
            public string DocumentId { get; set; }
        }
    }
}
