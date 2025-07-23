using System.Runtime.Serialization;

namespace SignNow.Net.Model
{
    public enum RedirectTarget
    {
        /// <summary>
        /// opens the link in the new tab
        /// </summary>
        [EnumMember(Value = "blank")]
        Blank,

        /// <summary>
        /// opens the link in the same tab.
        /// </summary>
        [EnumMember(Value = "self")]
        Self
    }
}
