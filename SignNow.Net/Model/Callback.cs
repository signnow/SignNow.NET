using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net._Internal.Helpers.Converters;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// RequestContent could be different based on event type.
    /// This model could represent any request content (CallbackContentAllFields) or specific one - any other type inherited from EventContentCallbackBase
    /// </summary>
    /// <typeparam name="T">Model from EventContentCallbackModels</typeparam>
    public class Callback<T> : CallbackBase where T : IEventContentCallback
    {
        [JsonProperty("request_content")]
        public CallbackRequestContent<T> RequestContent { get; set; }
    }

    /// <summary>
    /// Represents a webhook callback event in the SignNow system.
    /// </summary>
    public class CallbackBase
    {
        /// <summary>
        /// Unique identifier of the callback.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the application that owns the callback.
        /// </summary>
        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        /// <summary>
        /// The entity ID associated with the callback.
        /// </summary>
        [JsonProperty("entity_id")]
        public string EntityId { get; set; }

        /// <summary>
        /// The unique identifier of the event subscription.
        /// </summary>
        [JsonProperty("event_subscription_id")]
        public string EventSubscriptionId { get; set; }

        /// <summary>
        /// Indicates whether the event subscription is active.
        /// </summary>
        [JsonProperty("event_subscription_active")]
        public bool EventSubscriptionActive { get; set; }

        /// <summary>
        /// The entity type (e.g., "document", "user", "document_group", "template").
        /// </summary>
        [JsonProperty("entity_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventSubscriptionEntityType EntityType { get; set; }

        /// <summary>
        /// The specific event name that triggered the callback.
        /// </summary>
        [JsonProperty("event_name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType EventName { get; set; }

        /// <summary>
        /// The callback URL that was triggered.
        /// </summary>
        [JsonProperty("callback_url")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri CallbackUrl { get; set; }

        /// <summary>
        /// HTTP request method used for the callback.
        /// </summary>
        [JsonProperty("request_method")]
        public string RequestMethod { get; set; }

        /// <summary>
        /// The duration of the callback execution in seconds.
        /// </summary>
        [JsonProperty("duration")]
        public double Duration { get; set; }

        /// <summary>
        /// The timestamp when the callback request started (Unix timestamp).
        /// </summary>
        [JsonProperty("request_start_time")]
        public long RequestStartTime { get; set; }

        /// <summary>
        /// The timestamp when the callback request ended (Unix timestamp).
        /// </summary>
        [JsonProperty("request_end_time")]
        public long RequestEndTime { get; set; }

        /// <summary>
        /// The HTTP headers sent with the callback request.
        /// </summary>
        [JsonProperty("request_headers")]
        [JsonConverter(typeof(ObjectOrEmptyArrayConverter))]
        public EventAttributeHeaders RequestHeaders { get; set; }

        /// <summary>
        /// The response content received from the callback URL.
        /// </summary>
        [JsonProperty("response_content")]
        public string ResponseContent { get; set; }

        /// <summary>
        /// The HTTP response status code received from the callback URL.
        /// </summary>
        [JsonProperty("response_status_code")]
        public int ResponseStatusCode { get; set; }

        /// <summary>
        /// Email address of the owner of the event subscription.
        /// </summary>
        [JsonProperty("event_subscription_owner_email")]
        public string EventSubscriptionOwnerEmail { get; set; }
    }
}
