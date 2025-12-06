using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net._Internal.Helpers.Converters;
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
        /// The content sent in the callback request.
        /// </summary>
        [JsonProperty("request_content")]
        public CallbackRequestContent RequestContent { get; set; }

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

    /// <summary>
    /// Represents the content sent in a callback request.
    /// </summary>
    public class CallbackRequestContent
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
        public CallbackContentData Content { get; set; }
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

    /// <summary>
    /// Represents the content data in a callback.
    /// </summary>
    public class CallbackContentData
    {
        /// <summary>
        /// The document ID.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// The template ID.
        /// </summary>
        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        /// <summary>
        /// The invite ID.
        /// </summary>
        [JsonProperty("invite_id")]
        public string InviteId { get; set; }

        /// <summary>
        /// The signer information.
        /// </summary>
        [JsonProperty("signer")]
        public string Signer { get; set; }

        /// <summary>
        /// The status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The old invite unique ID.
        /// </summary>
        [JsonProperty("old_invite_unique_id")]
        public string OldInviteUniqueId { get; set; }

        /// <summary>
        /// The group ID.
        /// </summary>
        [JsonProperty("group_id")]
        public string GroupId { get; set; }

        /// <summary>
        /// The group name.
        /// </summary>
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        /// <summary>
        /// The group invite information.
        /// </summary>
        [JsonProperty("group_invite")]
        public string GroupInvite { get; set; }

        /// <summary>
        /// The group invite ID.
        /// </summary>
        [JsonProperty("group_invite_id")]
        public string GroupInviteId { get; set; }

        /// <summary>
        /// The document name.
        /// </summary>
        [JsonProperty("document_name")]
        public string DocumentName { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The initiator ID.
        /// </summary>
        [JsonProperty("initiator_id")]
        public string InitiatorId { get; set; }

        /// <summary>
        /// The initiator email.
        /// </summary>
        [JsonProperty("initiator_email")]
        public string InitiatorEmail { get; set; }

        /// <summary>
        /// The viewer user unique ID.
        /// </summary>
        [JsonProperty("viewer_user_unique_id")]
        public string ViewerUserUniqueId { get; set; }
    }
}
