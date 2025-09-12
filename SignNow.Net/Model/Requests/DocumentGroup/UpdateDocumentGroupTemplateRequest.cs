using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Request model for updating document group template
    /// </summary>
    public class UpdateDocumentGroupTemplateRequest : JsonHttpContent
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
        /// Specifies the action to be taken upon invite completion.
        /// </summary>
        [JsonProperty("email_action_on_complete")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmailActionsType EmailActionOnComplete { get; set; }
    }
}
