using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.Requests
{
    public class EditEventSubscription : JsonHttpContent
    {
        public EditEventSubscription(EventType eventType, string entityId, string subscriptionId, Uri callbackUrl)
        {
            Guard.ArgumentNotNull(callbackUrl, nameof(callbackUrl));
            Id = subscriptionId.ValidateId();
            EntityId = entityId.ValidateId();
            Event = eventType;
            Attributes.CallbackUrl = callbackUrl;
        }

        /// <summary>
        /// Identity of Event
        /// </summary>
        [JsonIgnore]
        public string Id { get; private set; }

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
        public EventAttributes Attributes { get; set; } = new EventAttributes();

        /// <summary>
        /// Enables the HMAC security logic.
        /// </summary>
        [JsonProperty("secret_key", NullValueHandling = NullValueHandling.Ignore)]
        public string SecretKey { get; set; }
    }
}
