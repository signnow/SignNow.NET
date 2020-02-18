using Newtonsoft.Json;

namespace SignNow.Net.Internal.Model.FieldTypes
{
    /// <summary>
    /// Represents SignNow field types: `Text box`, `Dropdown box`, `Date-Time picker`
    /// </summary>
    internal class TextField : BaseField
    {
        /// <summary>
        /// Raw text value of the field.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Returns text value of <see cref="Data"/> field.
        /// </summary>
        public override string ToString() => Data;
    }
}
