using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class CreateOrRenameFolderRequest : JsonHttpContent
    {
        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
