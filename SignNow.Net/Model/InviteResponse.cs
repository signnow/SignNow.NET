using System;
using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class InviteResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }
    }
}
