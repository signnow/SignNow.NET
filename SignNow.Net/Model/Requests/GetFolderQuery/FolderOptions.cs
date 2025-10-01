using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests.GetFolderQuery
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SubFolders
    {
        [EnumMember(Value = "0")]
        DoNotShow,

        [EnumMember(Value = "1")]
        Show
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntityType
    {
        [EnumMember(Value = "all")]
        All,

        [EnumMember(Value = "document")]
        Document,

        [EnumMember(Value = "document-group")]
        DocumentGroup,
    }
}
