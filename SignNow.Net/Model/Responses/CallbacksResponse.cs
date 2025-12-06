using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response containing a list of callback events with metadata.
    /// This follows the v2 API response structure with data and meta fields.
    /// </summary>
    public class CallbacksResponse
    {
        /// <summary>
        /// The list of callback events.
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyList<Callback> Data { get; set; }

        /// <summary>
        /// Metadata information including pagination details.
        /// </summary>
        [JsonProperty("meta")]
        public MetaInfo Meta { get; set; }
    }
}
