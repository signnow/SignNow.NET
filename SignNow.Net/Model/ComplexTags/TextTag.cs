using Newtonsoft.Json;

namespace SignNow.Net.Model.ComplexTags
{
    public class TextTag : ComplexTagWithLabel
    {
        /// <inheritdoc />
        public override FieldType Type { get; protected set; } = FieldType.Text;

        /// <summary>
        /// Editable text (optional) that appears in the field when the signer opens the document, e.g. Lucy
        /// </summary>
        [JsonProperty("prefilled_text", NullValueHandling = NullValueHandling.Ignore)]
        public string PrefilledText { get; set; }
    }
}
