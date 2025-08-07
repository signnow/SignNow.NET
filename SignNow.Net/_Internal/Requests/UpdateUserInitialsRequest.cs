using Newtonsoft.Json;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// Request for updating user initials with image data
    /// </summary>
    internal class UpdateUserInitialsRequest : JsonHttpContent
    {
        /// <summary>
        /// Base64 encoded image data for the user's initials
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
