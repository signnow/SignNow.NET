using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Request to create a document group directly from a document group template.
    /// </summary>
    public class CreateDocumentGroupFromTemplateRequest : JsonHttpContent
    {
        /// <summary>
        /// Name for the new document group.
        /// </summary>
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        /// <summary>
        /// Client-side timestamp of the request.
        /// </summary>
        [JsonProperty("client_timestamp")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime ClientTimestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ID of the folder to create the document group in. Optional.
        /// </summary>
        [JsonProperty("folder_id", NullValueHandling = NullValueHandling.Ignore)]
        public string FolderId { get; set; }
    }
}
