using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.ComplexTags

{
    public class HyperlinkTag : ComplexTagWithLabel
    {
        /// <inheritdoc />
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public override FieldType Type { get; protected set; } = FieldType.Hyperlink;

        /// <summary>
        /// Page number where is located hyperlink
        /// </summary>
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Hyperlink unique id
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Link { get; set; }

        /// <summary>
        /// Hyperlink hint
        /// </summary>
        [JsonProperty("hint")]
        public string Hint { get; set; }
    }
}
