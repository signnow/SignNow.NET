using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Model.Requests
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GetFolderOptions
    {
        [JsonProperty("filters")]
        public FolderFilters Filters { get; set; }

        [JsonProperty("sortby")]
        public FolderSort SortBy { get; set; }

        [JsonProperty("limit")]
        private Dictionary<string, int> InternalLimit { get; set; }

        [JsonProperty("offset")]
        private Dictionary<string, int> InternalOffset { get; set; }

        [JsonProperty("subfolder-data")]
        private Dictionary<string, SubFolders> InternalSubfolderData { get; set; }

        /// <summary>
        /// Displays specified number of documents;
        /// Min limit is 0 (no documents will be shown), Max limit is 100.
        /// </summary>
        [JsonIgnore]
        public int Limit
        {
            get => InternalLimit.Values.FirstOrDefault();
            set
            {
                if (value >= 0)
                    InternalLimit = new Dictionary<string, int> {{"limit", value > 100 ? 100 : value}};
            }
        }

        /// <summary>
        /// Displays documents from specified position.
        /// </summary>
        [JsonIgnore]
        public int Offset
        {
            get => InternalOffset.Values.FirstOrDefault();
            set => InternalOffset = new Dictionary<string, int> {{"offset", value > 0 ? value : 0}};
        }

        /// <summary>
        /// Defines whether sub-folders of the given folder are displayed in the response.
        /// <remarks>
        /// Values: <see cref="SubFolders.Show"/> - yes, displayed, <see cref="SubFolders.DoNotShow"/> - no, don't show.
        /// </remarks>
        /// </summary>
        [JsonIgnore]
        public SubFolders SubfolderData
        {
            get => InternalSubfolderData.Values.FirstOrDefault();
            set => InternalSubfolderData = new Dictionary<string, SubFolders> {{"subfolder-data", value}};
        }

        /// <summary>
        /// Converts <see cref="FolderFilters"/> to query sting
        /// </summary>
        /// <returns></returns>
        public string ToQueryString()
        {
            var filters = JsonConvert.SerializeObject(Filters);
            var sortBy = JsonConvert.SerializeObject(SortBy);
            var limit = JsonConvert.SerializeObject(InternalLimit);
            var offset = JsonConvert.SerializeObject(InternalOffset);
            var subfolders = JsonConvert.SerializeObject(InternalSubfolderData);

            var filterQuery = BuildQueryFromJson(filters, "filters={0}&filter-values={1}");
            var sortByQuery = BuildQueryFromJson(sortBy, "sortby={0}&order={1}");
            var limitQuery = BuildQueryFromJson(limit,"{0}={1}");
            var offsetQuery = BuildQueryFromJson(offset,"{0}={1}");
            var subfoldersQuery = BuildQueryFromJson(subfolders,"{0}={1}");

            var options = new List<string>
                    (new []{filterQuery, sortByQuery, limitQuery, offsetQuery, subfoldersQuery})
                .Where(d => d.Length != 0).ToList();

            return string.Join("&", options);
        }

        private static string BuildQueryFromJson(string json, string querySchema)
        {
            var toDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);

            if (toDictionary == null)
                return string.Empty;

            var query = toDictionary
                .Select(k => string.Format(CultureInfo.InvariantCulture, querySchema, k.Key, k.Value));

            return string.Join("&", query);
        }
    }
}
