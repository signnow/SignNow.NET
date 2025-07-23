using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    public class EmbeddedInviteLinkResponse
    {
        [JsonProperty("data")]
        internal Dictionary<string, Uri> Data { get; set; }

        /// <summary>
        /// Returns the embedded invite link data for newly created embedded invite.
        /// </summary>
        [JsonIgnore]
        public Uri Link => Data["link"];
    }
}
