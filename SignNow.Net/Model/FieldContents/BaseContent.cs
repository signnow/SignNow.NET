using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.FieldContents
{
    /// <summary>
    /// Basic SignNow field content.
    /// </summary>
    public abstract class BaseContent : ISignNowContent
    {
        /// <summary>
        /// Identity of field.
        /// </summary>
        [JsonProperty("id")]
        public string Id  {get; set; }

        /// <summary>
        /// Identity of User which fulfilled the field.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The page number of the document.
        /// </summary>
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        /// <inheritdoc />
        public abstract object GetValue();
    }
}
