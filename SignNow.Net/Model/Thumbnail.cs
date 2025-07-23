using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents thumbnails of the document.
    /// </summary>
    public class Thumbnail
    {
        /// <summary>
        /// Uri for small document preview image size.
        /// A4 (210 x 297 mm) ~ 85 x 110 px @ 10 ppi
        /// </summary>
        [JsonProperty("small")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Small { get; internal set; }

        /// <summary>
        /// Uri for medium document preview image size
        /// A4 (210 x 297 mm) ~ 340 x 440 px @ 40 ppi
        /// </summary>
        [JsonProperty("medium")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Medium { get; internal set; }

        /// <summary>
        /// Uri for large document preview image size.
        /// A4 (210 x 297 mm) ~ 890 x 1151 px @ 104 ppi
        /// </summary>
        [JsonProperty("large")]
        [JsonConverter(typeof(StringToUriJsonConverter))]
        public Uri Large { get; internal set; }
    }
}
