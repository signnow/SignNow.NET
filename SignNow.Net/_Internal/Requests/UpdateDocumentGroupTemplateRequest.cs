using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Model;
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
        /// List of document IDs in the document group template
        /// </summary>
        [JsonProperty("order")]
        public IList<string> Order { get; set; } = new List<string>();

        /// <summary>
        /// Name of the document group template
        /// </summary>
        [JsonProperty("template_group_name")]
        public string TemplateGroupName { get; set; }

        /// <summary>
        /// Specifies the action to be taken upon invite completion
        /// </summary>
        [JsonProperty("email_action_on_complete")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmailActionsType EmailActionOnComplete { get; set; }

        /// <summary>
        /// Creates a new instance of UpdateDocumentGroupTemplateRequest from the public request model
        /// </summary>
        /// <param name="request">Public request model</param>
        public UpdateDocumentGroupTemplateRequest(SignNow.Net.Model.Requests.DocumentGroup.UpdateDocumentGroupTemplateRequest request)
        {
            Order = request.Order;
            TemplateGroupName = request.TemplateGroupName;
            EmailActionOnComplete = request.EmailActionOnComplete;
        }
    }
}
