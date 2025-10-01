using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Authentication type for signer authorization.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthenticationType
    {
        /// <summary>
        /// Password authentication
        /// </summary>
        [EnumMember(Value = "password")]
        Password,

        /// <summary>
        /// Phone call authentication
        /// </summary>
        [EnumMember(Value = "phone_call")]
        PhoneCall,

        /// <summary>
        /// SMS authentication
        /// </summary>
        [EnumMember(Value = "sms")]
        Sms
    }
}
