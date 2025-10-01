using System.Runtime.Serialization;

namespace SignNow.Net.Model
{
    public enum GrantType
    {
        [EnumMember(Value = "password")]
        Password,

        [EnumMember(Value = "refresh_token")]
        RefreshToken,

        [EnumMember(Value = "authorization_code")]
        AuthorizationCode
    }
}
