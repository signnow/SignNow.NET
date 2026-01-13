using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Represents response from signNow API for document fields data request.
    /// </summary>
    public class DocumentFieldsResponse
    {
        /// <summary>
        /// Array of field data objects.
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyCollection<DocumentFieldData> Data { get; set; }

        /// <summary>
        /// Metadata information including pagination.
        /// </summary>
        [JsonProperty("meta")]
        public MetaInfo Meta { get; set; }
    }

    /// <summary>
    /// Represents individual field data from a completed document.
    /// </summary>
    public class DocumentFieldData
    {
        /// <summary>
        /// Unique identifier of the field.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the field.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Type of the field.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Value of the field. Can be null if the field is not filled.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
