using System;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents a response from the update user initials endpoint.
    /// </summary>
    public class UpdateUserInitialsResponse
    {
        /// <summary>
        /// Unique identifier of the created initial image.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Width of the initial image in pixels.
        /// </summary>
        [JsonProperty("width")]
        [JsonConverter(typeof(StringToIntJsonConverter))]
        public int Width { get; set; }

        /// <summary>
        /// Height of the initial image in pixels.
        /// </summary>
        [JsonProperty("height")]
        [JsonConverter(typeof(StringToIntJsonConverter))]
        public int Height { get; set; }

        /// <summary>
        /// Timestamp when the initial was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }
    }
}
