using Newtonsoft.Json;

namespace SignNow.Net.Model.FieldTypes
{
    /// <summary>
    /// Represents SignNow field types: `Checkbox`
    /// </summary>
    public class CheckboxField : BaseField
    {
        /// <summary>
        /// Email of user who fulfilled the field.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

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
