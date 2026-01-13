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

        public class EventAttributes
        {
            /// <summary>
            /// Determines whether to keep access_token in the payload.
            /// If true, then we should delete access_token key from payload.
            /// If false, keep the access_token in payload attributes
            /// Default: true
            /// </summary>
            [JsonProperty("delete_access_token")]
            public bool DeleteAccessToken { get; set; } = true;

            /// <summary>
            /// URL of external callback
            /// </summary>
            [JsonProperty("callback")]
            [JsonConverter(typeof(StringToUriJsonConverter))]
            public Uri CallbackUrl { get; set; }

            /// <summary>
            /// If true, 1.2 tls version will be used. If false, default tls version will be used.
            /// </summary>
            [JsonProperty("use_tls_12")]
            public bool UseTls12 { get; set; }

            /// <summary>
            /// Unique ID of external system. It is stored in "api_integrations" database table.
            /// </summary>
            [JsonProperty("integration_id", NullValueHandling = NullValueHandling.Ignore)]
            public string IntegrationId { get; internal set; }

            /// <summary>
            /// If true, JSON, which is sent callback,
            /// will consist document id as query string parameter and as a part of "callback_url" parameter.
            /// </summary>
            [JsonProperty("docid_queryparam")]
            public bool DocIdQueryParam { get; set; }

            /// <summary>
            /// Optional headers. You can add any parameters to "headers"
            /// </summary>
            [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
            public EventAttributeHeaders Headers { get; set; }

            /// <summary>
            /// Whether the payload of the webhook should include metadata.
            /// </summary>
            [JsonProperty("include_metadata", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IncludeMetadata { get; set; }

            /// <summary>
            /// Enables the HMAC security logic
            /// </summary>
            [JsonProperty("secret_key", NullValueHandling = NullValueHandling.Ignore)]
            public string SecretKey { get; set; }
        }
    }
}
