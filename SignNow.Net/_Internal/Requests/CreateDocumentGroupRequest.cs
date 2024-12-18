using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    internal class CreateDocumentGroupRequest : JsonHttpContent
    {
        /// <summary>
        /// A list of document unique ids from that Document Group will be created.
        /// </summary>
        [JsonProperty("document_ids")]
        public List<string> DocumentIds { get; set; } = new List<string>();

        /// <summary>
        /// Name of the Document Group.
        /// </summary>
        [JsonProperty("group_name")]
        public string GroupName { get; set; }

        public CreateDocumentGroupRequest(IEnumerable<SignNowDocument>documents)
        {
            foreach (var document in documents)
            {
               DocumentIds.Add(document.Id);
            }
        }
    }
}
