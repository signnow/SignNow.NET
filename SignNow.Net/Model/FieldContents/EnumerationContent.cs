using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.FieldContents
{
    /// <summary>
    /// Represents an element of Enumeration options
    /// for `Dropdown` field type.
    /// </summary>
    public class EnumerationContent
    {
        /// <summary>
        /// Unique identifier of field.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Unique identifier of enumeration element.
        /// </summary>
        [JsonProperty("enumeration_id")]
        public string EnumerationId { get; set; }

        /// <summary>
        /// Enumeration value.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        /// Timestamp enumeration was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// Timestamp enumeration was updated.
        /// </summary>
        [JsonProperty("updated")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Updated { get; set; }
    }
}
