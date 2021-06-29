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

        [JsonProperty("document-name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SortOrder? DocumentName { get; private set; }

        /// <summary>
        /// Construct Folder sort object.
        /// </summary>
        /// <param name="sortBy">Sort documents by <see cref="SortByParam.Created"/>, <see cref="SortByParam.Updated"/> date or by <see cref="SortByParam.DocumentName"/>.</param>
        /// <param name="order">Order documents in <see cref="SortOrder.Ascending"/> or <see cref="SortOrder.Descending"/> way.</param>
        public FolderSort(SortByParam sortBy, SortOrder order)
        {
            if (sortBy == SortByParam.Created)
                Created = order;
            else if (sortBy == SortByParam.Updated)
                Updated = order;
            else if (sortBy == SortByParam.DocumentName)
                DocumentName = order;
        }
    }

    /// <summary>
    /// Options to set sort documents in Folder by create, update date or document name.
    /// </summary>
    public enum SortByParam
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
        Updated,

        /// <summary>
        /// Sorts documents by name.
        /// </summary>
        [EnumMember(Value = "document-name")]
        DocumentName
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
