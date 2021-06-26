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

        /// <summary>
        /// Converts <see cref="FolderFilters"/> to query sting
        /// </summary>
        /// <returns></returns>
        public string ToQueryString()
        {
            var filters = JsonConvert.SerializeObject(Filters);
            var sortBy = JsonConvert.SerializeObject(SortBy);

            string filterQuery = default;
            string sortByQuery = default;

            if (!string.IsNullOrEmpty(filters) || !filters.ToUpperInvariant().Equals("NULL"))
            {
                filterQuery = BuildQueryFromJson(filters, "filters={0}&filter-values={1}");
                sortByQuery = BuildQueryFromJson(sortBy, "sortby={0}&order={1}");
            }

            var options = new List<string>();
            if (!string.IsNullOrEmpty(filterQuery))
                options.Add(filterQuery);

            if (!string.IsNullOrEmpty(sortByQuery))
                options.Add(sortByQuery);

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
