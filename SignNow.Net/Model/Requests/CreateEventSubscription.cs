using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;

namespace SignNow.Net.Model.Requests
{
    public class CreateEventSubscription : JsonHttpContent
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
        [JsonProperty("secret_key", NullValueHandling = NullValueHandling.Ignore)]
        public string SecretKey { get; set; }

        public CreateEventSubscription()
        {
            // Guard.ArgumentNotNull(eventSubscription.JsonAttributes.CallbackUrl, nameof(eventSubscription.JsonAttributes.CallbackUrl));
            //
            // Event = eventSubscription.Event;
            // EntityId = eventSubscription.EntityUid.ValidateId();
            // Attributes = eventSubscription.JsonAttributes;
            // SecretKey = eventSubscription.JsonAttributes.SecretKey;
            // eventSubscription.JsonAttributes.SecretKey = String.Empty;
        }
    }
}
