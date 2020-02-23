using System.ComponentModel;

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
        All,

        /// <summary>
        /// Scope which garnted access to User secvice.
        /// </summary>
        User
    }
}
