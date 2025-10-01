using Newtonsoft.Json;

namespace SignNow.Net.Model.EditFields
{
    public class SignatureField: AbstractField
    {
        /// <inheritdoc />
        public override FieldType Type => FieldType.Signature;

        /// <summary>
        /// Field label.
        /// </summary>
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
    }
}