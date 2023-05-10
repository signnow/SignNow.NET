using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Helpers.Converters;

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
        public EventCreateAttributes Attributes { get; set; } = new EventCreateAttributes();

        /// <summary>
        /// Enables the HMAC security logic.
        /// </summary>
        [JsonProperty("secret_key", NullValueHandling = NullValueHandling.Ignore)]
        public string SecretKey { get; set; }

        public CreateEventSubscription(EventType eventType, string entityId, Uri callbackUrl)
        {
            Guard.ArgumentNotNull(callbackUrl, nameof(callbackUrl));

            Event = eventType;
            EntityId = entityId.ValidateId();
            Attributes.CallbackUrl = callbackUrl;
        }
    }

    public class EventCreateAttributes
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
        public string IntegrationId { get; set; }

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
        /// Enables the HMAC security logic
        /// </summary>
        [JsonProperty("secret_key", NullValueHandling = NullValueHandling.Ignore)]
        public string SecretKey { get; set; }
    }
}
