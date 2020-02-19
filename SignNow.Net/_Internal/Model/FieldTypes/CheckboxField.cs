using Newtonsoft.Json;

namespace SignNow.Net.Internal.Model.FieldTypes
{
    /// <summary>
    /// Represents SignNow field types: `Checkbox`
    /// </summary>
    internal class CheckboxField : BaseField
    {
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