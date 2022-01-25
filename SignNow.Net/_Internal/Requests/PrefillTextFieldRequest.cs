using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.EditFields;

namespace SignNow.Net.Internal.Requests
{
    internal class PrefillTextFieldRequest : IContent
    {
        /// <summary>
        /// Collections of <see cref="TextField"/> request options.
        /// </summary>
        [JsonProperty("fields")]
        internal List<PrefillText> Fields { get; set; } = new List<PrefillText>();

        public PrefillTextFieldRequest(IEnumerable<TextField> fields)
        {
            foreach (var field in fields)
            {
                Fields.Add(new PrefillText { FieldName = field.Name, PrefilledText = field.PrefilledText });
            }
        }

        public PrefillTextFieldRequest(TextField field)
        {
            Fields.Add(new PrefillText { FieldName = field.Name, PrefilledText = field.PrefilledText });
        }

        /// <summary>
        /// Creates Json Http Content from object
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
        }
    }

    internal class PrefillText
    {
        /// <summary>
        /// The unique field name that identifies the field.
        /// </summary>
        [JsonProperty("field_name")]
        public string FieldName { get; set; }

        /// <summary>
        /// The value that should appear on the document.
        /// </summary>
        [JsonProperty("prefilled_text")]
        public string PrefilledText { get; set; }
    }
}
