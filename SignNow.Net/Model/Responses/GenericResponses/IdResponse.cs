using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses.GenericResponses
{
    /// <summary>
    /// Represents response from signNow API with identity of the signNow object.
    /// </summary>
    public abstract class IdResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
