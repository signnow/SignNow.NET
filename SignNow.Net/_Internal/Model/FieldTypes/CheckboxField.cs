using Newtonsoft.Json;

namespace SignNow.Net.Internal.Model.FieldTypes
{
    /// <summary>
    /// Represents SignNow field types: `Checkbox`
    /// </summary>
    internal class CheckboxField : BaseField
    {
        // <summary>
        /// The page number of the document.
        /// </summary>
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Checkbox value (checked or unchecked).
        /// </summary>
        [JsonIgnore]
        public bool Data { get; set; }

        /// <summary>
        /// Returns text value of <see cref="Data"/> field.
        /// </summary>
        public override string ToString() => Data ? "1" : "";
    }
}
