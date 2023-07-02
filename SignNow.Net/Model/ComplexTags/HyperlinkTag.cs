using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.ComplexTags

{
    public class HyperlinkTag : ComplexTag
    {
        /// <inheritdoc />
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

        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Link { get; set; }

        /// <summary>
        /// Hyperlink label
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Hyperlink hint
        /// </summary>
        [JsonProperty("hint")]
        public string Hint { get; set; }
    }
}
