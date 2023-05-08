using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests
{
    public class CreateEventSubscriptionRequest
    {
        /// <summary>
        /// Event type.
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; }

        /// <summary>
        /// The unique ID of the event: "document_id", "user_id", "document_group_id", "template_id".
        /// </summary>
        [JsonProperty("entity_id")]
        public string EntityId { get; set; }

        /// <summary>
        /// Always only "callback"
        /// </summary>
        [JsonProperty("action")]
        public string Action = "callback";

        /// <summary>
        /// Event attributes.
        /// </summary>
        [JsonProperty("attributes")]
        public EventAttributes Attributes { get; set; }

        /// <summary>
        /// Enables the HMAC security logic.
        /// </summary>
        [JsonProperty("secret_key")]
        public string SecretKey { get; set; }

        public CreateEventSubscriptionRequest(EventSubscription eventSubscription)
        {
            Event = eventSubscription.Event;
        }
    }
}
