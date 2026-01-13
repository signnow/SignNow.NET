using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Authentication type for authentication info.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthenticationInfoType
    {
        /// <summary>
        /// Password authentication
        /// </summary>
        [EnumMember(Value = "password")]
        Password,

        /// <summary>
        /// Phone authentication
        /// </summary>
        [EnumMember(Value = "phone")]
        Phone
    }
}
