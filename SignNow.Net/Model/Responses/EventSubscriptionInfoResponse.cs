using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    public class EventSubscriptionInfoResponse
    {
        [JsonProperty("data")]
        public EventSubscription ResponseData { get; set; }
    }
}
