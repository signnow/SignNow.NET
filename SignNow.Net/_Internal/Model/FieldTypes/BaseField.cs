using Newtonsoft.Json;

namespace SignNow.Net.Internal.Model.FieldTypes
{
    /// <summary>
    /// Basic SignNow field.
    /// </summary>
    internal abstract class BaseField
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
        /// Email of user who fulfilled the field.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// The page number of the document.
        /// </summary>
        [JsonProperty("page_number")]
        public int PageNumber { get; set; }
    }
}