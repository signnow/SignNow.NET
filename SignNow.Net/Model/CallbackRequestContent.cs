using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents the content sent in a callback request.
    /// </summary>
    public class CallbackRequestContent<T> where T : EventContentCallbackBase
    {
        /// <summary>
        /// Metadata about the callback request.
        /// </summary>
        [JsonProperty("meta")]
        public CallbackRequestMeta Meta { get; set; }

        /// <summary>
        /// The actual content/payload of the callback.
        /// </summary>
        [JsonProperty("content")]
        public T Content { get; set; }
    }

    /// <summary>
    /// Represents metadata about the callback request.
    /// </summary>
    public class CallbackRequestMeta
    {
        /// <summary>
        /// The timestamp when the event occurred.
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// The event name.
        /// </summary>
        [JsonProperty("event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType Event { get; set; }

        /// <summary>
        /// The environment URL.
        /// </summary>
        [JsonProperty("environment")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Environment { get; set; }

        /// <summary>
        /// The ID of the user who initiated the action.
        /// </summary>
        [JsonProperty("initiator_id")]
        public string InitiatorId { get; set; }

        /// <summary>
        /// The callback URL.
        /// </summary>
        [JsonProperty("callback_url")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri CallbackUrl { get; set; }

        /// <summary>
        /// The access token (masked for security).
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
