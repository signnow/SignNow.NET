using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Request model for creating a document group template from a document group
    /// </summary>
    public class CreateDocumentGroupTemplateRequest : JsonHttpContent
    {
        /// <summary>
        /// Name for the Document Group Template
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Folder ID where the template will be created
        /// </summary>
        [JsonProperty("folder_id")]
        public string FolderId { get; set; }

        /// <summary>
        /// Whether to own the template as merged
        /// </summary>
        [JsonProperty("own_as_merged")]
        public bool? OwnAsMerged { get; set; }
    }
}
