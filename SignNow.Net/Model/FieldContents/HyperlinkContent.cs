using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.FieldContents
{
    /// <summary>
    /// Represents SignNow field types: `Hyperlink`
    /// </summary>
    public class HyperlinkContent : BaseContent
    {
        /// <summary>
        /// Email of user who fulfilled the field.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Hyperlink label.
        /// </summary>
        [JsonProperty("label")]
        public string Label {get; set; }

        /// <summary>
        /// Hyperlink field value <see cref="Uri"/>
        /// </summary>
        [JsonProperty("data")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Data { get; set; }

        /// <summary>
        /// Returns Hyperlink content as <see cref="Uri"/> string.
        /// </summary>
        public override string ToString() => Data.OriginalString;

        /// <inheritdoc />
        public override object GetValue() => Data;
    }
}
