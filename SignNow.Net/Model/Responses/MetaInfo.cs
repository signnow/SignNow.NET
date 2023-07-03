using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class MetaInfo
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; internal set; }
    }
}