using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class MoveDocumentRequest : JsonHttpContent
    {
        [JsonProperty("folder_id")]
        public string FolderId { get; set; }
    }
}
