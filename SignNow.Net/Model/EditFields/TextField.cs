using Newtonsoft.Json;

namespace SignNow.Net.Model.EditFields
{
    public class TextField: AbstractField
    {
        /// <inheritdoc />
        public override string Type => "text";

        /// <summary>
        /// Prefilled text value of the field.
        /// </summary>
        [JsonProperty("prefilled_text", NullValueHandling = NullValueHandling.Ignore)]
        public string PrefilledText { get; set; }

        /// <summary>
        /// Field label.
        /// </summary>
        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }
    }
}
