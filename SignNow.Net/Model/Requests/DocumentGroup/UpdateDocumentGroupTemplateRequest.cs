using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Request model for updating document group template
    /// </summary>
    public class UpdateDocumentGroupTemplateRequest : JsonHttpContent
    {
        /// <summary>
        /// Array of template IDs to add to the document group template
        /// </summary>
        [JsonProperty("template_ids_to_add")]
        public IList<string> TemplateIdsToAdd { get; set; } = new List<string>();

        /// <summary>
        /// Array of template IDs to remove from the document group template
        /// </summary>
        [JsonProperty("template_ids_to_remove")]
        public IList<string> TemplateIdsToRemove { get; set; } = new List<string>();

        /// <summary>
        /// Routing details as JSON string for the document group template
        /// </summary>
        [JsonProperty("routing_details")]
        public string RoutingDetails { get; set; }

        /// <summary>
        /// New name for the document group template
        /// </summary>
        [JsonProperty("template_group_name")]
        public string TemplateGroupName { get; set; }
    }
}
