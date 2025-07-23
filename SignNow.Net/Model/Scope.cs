using System.ComponentModel;
using System.Runtime.Serialization;

namespace SignNow.Net.Model
{
    /// <summary>
    /// The scope granted to the access token.
    /// </summary>
    public enum Scope
    {
        /// <summary>
        /// Scope which granted access to all services.
        /// </summary>
        [EnumMember(Value = "*")]
        All,

        /// <summary>
        /// Scope which granted access to User service.
        /// </summary>
        [EnumMember(Value = "user")]
        User
    }
}
