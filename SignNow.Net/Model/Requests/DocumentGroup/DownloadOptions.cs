using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    public class DownloadOptions : JsonHttpContent
    {
        /// <inheritdoc cref="SignNow.Net.Model.Requests.DocumentGroup.DownloadType"/>
        [JsonProperty("type")]
        public DownloadType DownloadType { get; set; } = DownloadType.Zip;

        /// <inheritdoc cref="SignNow.Net.Model.Requests.DocumentGroup.DocumentHistoryType"/>
        [JsonProperty("with_history")]
        public DocumentHistoryType WithHistory { get; set; } = DocumentHistoryType.NoHistory;

        /// <summary>
        /// The order of documents in the merged file. e.g: ordered list with signNow document ids.
        /// </summary>
        [JsonProperty("document_order", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> DocumentOrder { get; set; } = new List<string>();
    }
}
