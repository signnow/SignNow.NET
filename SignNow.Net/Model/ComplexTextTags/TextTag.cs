using Newtonsoft.Json;

namespace SignNow.Net.Model.ComplexTextTags
{
    public class TextTag : ComplexTagBase
    {
        /// <inheritdoc />
        public override FieldType Type { get; set; } = FieldType.Text;

        /// <summary>
        /// Editable text (optional)that appears in the field when the signer opens the document, e.g. Lucy
        /// </summary>
        [JsonProperty("prefilled_text", NullValueHandling = NullValueHandling.Ignore)]
        public string PrefilledText { get; set; }
    }
}
