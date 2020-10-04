using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;

namespace SignNow.Net.Internal.Requests
{
    internal class MergeDocumentRequest : IContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("document_ids")]
        public List<string> DocumentIds { get; set; } = new List<string>();

        /// <summary>
        /// Adds documents to be merged
        /// </summary>
        /// <param name="docs">Collection of documents</param>
        public void AddDocuments(IEnumerable<SignNowDocument> docs)
        {
            foreach (var doc in docs)
            {
                DocumentIds.Add(doc.Id.ValidateId());
            }
        }

        /// <summary>
        /// Creates Json Http Content from object
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(
                JsonConvert.SerializeObject(this),
                Encoding.UTF8, "application/json");
        }
    }
}
