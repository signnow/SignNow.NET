using Newtonsoft.Json;

namespace SignNow.Net.Model.FieldTypes
{
    /// <summary>
    /// Represents SignNow field types: `Text box`, `Dropdown box`, `Date-Time picker`
    /// </summary>
    public class TextField : BaseField
    {
        /// <summary>
        /// Email of user who fulfilled the field.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

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
