using System.Runtime.Serialization;

namespace SignNow.Net.Model.Requests.GetFolderQuery
{
    public enum SubFolders
    {
        [EnumMember(Value = "0")]
        DoNotShow,

        [EnumMember(Value = "1")]
        Show
    }
}
