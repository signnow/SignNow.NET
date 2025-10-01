using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class RenameDocumentGroupRequest : JsonHttpContent
    {
        [JsonProperty("group_name")]
        public string GroupName { get; set; }
    }
}
