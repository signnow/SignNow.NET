using Newtonsoft.Json;

namespace SignNow.Net.Interfaces
{
    public interface IFieldEditable
    {
        /// <summary>
        /// Page number where Text field located.
        /// </summary>
        [JsonProperty("page_number")]
        int PageNumber { get; set; }

        /// <summary>
        /// Type of the Field.
        /// </summary>
        [JsonProperty("type")]
        string Type { get; }

        /// <summary>
        /// The name of the Field.
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        string Name { get; set; }

        /// <summary>
        /// Signer role name.
        /// </summary>
        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        string Role { get; set;  }

        /// <summary>
        /// Is field required or not.
        /// </summary>
        [JsonProperty("required")]
        bool Required { get; set; }

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
    }
}
