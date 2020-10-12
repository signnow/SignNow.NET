using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from SignNow API for Document history request.
    /// </summary>
    [JsonObject]
    public class DocumentHistoryResponse
    {
        /// <summary>
        /// Unique identifier of history item.
        /// </summary>
        [JsonProperty("unique_id")]
        public string Id { get; set; }

        /// <summary>
        /// Identity of the document.
        /// </summary>
        [JsonProperty("document_id")]
        public string DocumentId { get; set; }

        /// <summary>
        /// Identity of user who owned this document.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Email of document owner.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Client Application name
        /// </summary>
        [JsonProperty("client_app_name")]
        public string AppName { get; set; }

        /// <summary>
        /// Actor's IP address
        /// </summary>
        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

        /// <summary>
        /// Name of event
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; set; }

        /// <summary>
        /// An origin of document
        /// </summary>
        [JsonProperty("origin")]
        public string Origin { get; set; }

        /// <summary>
        /// Version of signed document (incremented after each signature addition)
        /// </summary>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Client Application Timestamp of document creation.
        /// </summary>
        [JsonProperty("client_timestamp")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime ClientTimestamp { get; set; }

        /// <summary>
        /// Timestamp of document creation.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// An ID of specific field
        /// </summary>
        [JsonProperty("field_id")]
        public string FieldId { get; set; }

        /// <summary>
        /// An ID of specific element
        /// </summary>
        [JsonProperty("element_id")]
        public string ElementId { get; set; }

        /// <summary>
        /// Field attributes (e.g. font, style, size etc.)
        /// </summary>
        [JsonProperty("json_attributes")]
        public string JsonAttributes { get; set; }
    }
}
