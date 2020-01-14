using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Internal.Model
{
    /// <summary>
    /// Represent document signer roles.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Role unique id.
        /// </summary>
        [JsonProperty("unique_id")]
        public string Id { get; set; }

        /// <summary>
        /// Role signing order.
        /// </summary>
        [JsonProperty("signing_order")]
        [JsonConverter(typeof(StringToIntJsonConverter))]
        public int SigningOrder { get; set; }

        /// <summary>
        /// Role signing name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
