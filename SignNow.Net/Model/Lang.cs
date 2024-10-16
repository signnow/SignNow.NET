using System.Runtime.Serialization;

namespace SignNow.Net.Model
{
    public enum Lang
    {
        [EnumMember(Value = "en")]
        English,

        [EnumMember(Value = "es")]
        Spanish,

        [EnumMember(Value = "fr")]
        French
    }
}
