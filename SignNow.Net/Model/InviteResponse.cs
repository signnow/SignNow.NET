using System;
using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class InviteResponse
    {
        /// <summary>
        /// Identity of invite request.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
