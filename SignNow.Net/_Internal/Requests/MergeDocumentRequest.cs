using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class MergeDocumentRequest : JsonHttpContent
    {
        /// <summary>
        /// The name of the document that will be created and written to.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// A list of document unique ids that will be merged.
        /// </summary>
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
    }
}
