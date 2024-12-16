using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    public class CopyDocumentGroupRequest : JsonHttpContent
    {
        /// <summary>
        /// The name of the new document group copy.
        /// </summary>
        [JsonProperty("document_group_name")]
        public string DocumentGroupName { get; set; }

        /// <summary>
        /// Timestamp document was created.
        /// </summary>
        [JsonProperty("client_timestamp")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime ClientTimestamp { get; set; } = DateTime.UtcNow;
    }
}
