using Newtonsoft.Json;

namespace SignNow.Net.Internal.Model
{
    /// <summary>
    /// Field's attributes.
    /// </summary>
    internal class FieldJsonAttributes
    {
        /// <summary>
        /// The page number of the document.
        /// </summary>
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        /// <summary>
        /// X coordinate of the field.
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// Y coordinate of the field.
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }

        /// <summary>
        /// Width of the field.
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Height of the field.
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Is field required or not.
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }

        /// <summary>
        /// Prefilled text value of the field.
        /// </summary>
        [JsonProperty("prefilled_text")]
        public string PrefilledText { get; set; }

        /// <summary>
        /// Field label.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Field name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
