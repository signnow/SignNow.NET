using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Adds values to fields that the Signers can later edit when they receive the document for signature.
    /// Works only with Text field types.
    /// </summary>
    public class PrefillTextField
    {
        /// <summary>
        /// The unique field name that identifies the field.
        /// Can be found in the name parameter of the field in response from GET /document/{{document_id}}
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
