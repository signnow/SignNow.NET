using Newtonsoft.Json;
using SignNow.Net.Model.Responses.GenericResponses;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from signNow API for create invite request.
    /// </summary>
    public class InviteResponse : IdResponse
    {
        /// <summary>
        /// Role-based invite status
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
