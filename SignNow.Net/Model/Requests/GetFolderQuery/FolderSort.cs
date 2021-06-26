using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model.Requests.GetFolderQuery
{
    /// <summary>
    /// Sorts documents by creation or update date in descending or ascending order.
    /// </summary>
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public sealed class FolderSort
    {
        [JsonProperty("updated")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder? Updated { get; private set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder? Created { get; private set; }

        /// <summary>
        /// Construct Folder sort object.
        /// </summary>
        /// <param name="sortBy">Sort documents by <see cref="SortByDate.Created"/> or <see cref="SortByDate.Updated"/> date.</param>
        /// <param name="order">Order documents in <see cref="SortOrder.Ascending"/> or <see cref="SortOrder.Descending"/> way.</param>
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

    /// <summary>
    /// Options to set sort documents in Folder by create or update date.
    /// </summary>
    public enum SortByDate
    {
        /// <summary>
        /// Sorts documents by creation date.
        /// </summary>
        [EnumMember(Value = "created")]
        Created,

        /// <summary>
        /// Sorts documents by update date.
        /// </summary>
        [EnumMember(Value = "updated")]
        Updated
    }

    /// <summary>
    /// Options to set sort documents in Folder in descending or ascending order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// The order of the documents is ascending.
        /// </summary>
        [EnumMember(Value = "asc")]
        Ascending,

        /// <summary>
        /// The order of the documents is descending.
        /// </summary>
        [EnumMember(Value = "desc")]
        Descending
    }
}
