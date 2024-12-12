using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    public class MoveDocumentGroupRequest: JsonHttpContent
    {
        /// <summary>
        /// ID of the folder to move the document group to. Allowed folder types: Documents, Archive, Shared Documents folders.
        /// </summary>
        [JsonProperty("folder_id")]
        internal string FolderId { get; set; }

        /// <summary>
        /// Whether to move shared documents that are in this document group. With this parameter, a document group can only be moved to trash.
        /// </summary>
        [JsonProperty("with_shared_documents")]
        internal bool WithSharedDocuments { get; set; }
    }
}
