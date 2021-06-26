using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests.GetFolderQuery
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public sealed class FolderSort
    {
        [JsonProperty("updated")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder? Updated { get; private set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder? Created { get; private set; }

        public FolderSort(SortByDate sortBy, SortOrder order)
        {
            if (sortBy == SortByDate.Created)
            {
                Created = order;
            }
            else
            {
                Updated = order;
            }
        }
    }

    public enum SortByDate
    {
        [EnumMember(Value = "created")]
        Created,

        [EnumMember(Value = "updated")]
        Updated
    }

    public enum SortOrder
    {
        [EnumMember(Value = "asc")]
        Ascending,

        [EnumMember(Value = "desc")]
        Descending
    }
}
