using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.DocumentGroup;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// Internal request class for updating document group template
    /// </summary>
    internal class UpdateDocumentGroupTemplateRequest : JsonHttpContent
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

        /// <summary>
        /// Creates a new instance of UpdateDocumentGroupTemplateRequest from the public request model
        /// </summary>
        /// <param name="request">Public request model</param>
        public UpdateDocumentGroupTemplateRequest(SignNow.Net.Model.Requests.DocumentGroup.UpdateDocumentGroupTemplateRequest request)
        {
            TemplateIdsToAdd = request.TemplateIdsToAdd;
            TemplateIdsToRemove = request.TemplateIdsToRemove;
            RoutingDetails = request.RoutingDetails;
            TemplateGroupName = request.TemplateGroupName;
        }
    }
}
