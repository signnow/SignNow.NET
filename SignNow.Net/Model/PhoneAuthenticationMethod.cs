using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Allowed methods for phone authentication.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PhoneAuthenticationMethod
    {
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
