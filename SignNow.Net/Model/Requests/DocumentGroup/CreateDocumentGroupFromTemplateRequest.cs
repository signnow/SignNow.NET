using Newtonsoft.Json;

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
        /// Client-side timestamp of the request. Optional.
        /// </summary>
        [JsonProperty("client_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientTimestamp { get; set; }

        /// <summary>
        /// ID of the folder to create the document group in. Optional.
        /// </summary>
        [JsonProperty("folder_id", NullValueHandling = NullValueHandling.Ignore)]
        public string FolderId { get; set; }
    }
}
