using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class EventHistoryListResponse
    {
        [JsonProperty("meta")]
        public MetaInfo Meta { get; set; }
    }
}
