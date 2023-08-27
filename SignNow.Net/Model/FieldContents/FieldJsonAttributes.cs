using Newtonsoft.Json;

namespace SignNow.Net.Model.FieldContents
{
    /// <summary>
    /// Field's attributes.
    /// </summary>
    public class FieldJsonAttributes
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

        /// <summary>
        /// Use the current date when the recipient is signing the document as a Date field value.
        /// </summary>
        [JsonProperty("lock_to_sign_date", NullValueHandling = NullValueHandling.Ignore)]
        public bool LockToSignDate { get; set; }

        /// <summary>
        /// ID of regular expression validator supported by signNow.
        /// </summary>
        [JsonProperty("validator_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ValidatorId { get; set; }
    }
}
