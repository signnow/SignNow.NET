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

        [JsonProperty("with_team_documents")]
        private Dictionary<string, bool> InternalWithTeamDocument { get; set; }

        [JsonProperty("include_documents_subfolders")]
        private Dictionary<string, bool> InternalIncludeDocumentsSubfolder { get; set; }

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
        /// Allows to display "Team Documents" folders. Allowed values: true, false.
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public bool WithTeamDocuments
        {
            get => InternalWithTeamDocument.Values.FirstOrDefault();
            set => InternalWithTeamDocument = new Dictionary<string, bool> {{"with_team_documents", value}};
        }

        /// <summary>
        /// Allows to hide subfolders and display all documents from those subfolders in the parent folder.
        /// Parameter works only for "Documents" and "Template" folder and their children.
        /// Default value: true
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public bool IncludeDocumentsSubfolder
        {
            get => InternalIncludeDocumentsSubfolder.Values.FirstOrDefault();
            set => InternalIncludeDocumentsSubfolder = new Dictionary<string, bool> {{"include_documents_subfolders", value}};
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
            var withTeamDocs = JsonConvert.SerializeObject(InternalWithTeamDocument);
            var includeDocsSubfolder = JsonConvert.SerializeObject(InternalIncludeDocumentsSubfolder);

            var filterQuery = BuildQueryFromJson(filters, "filters={0}&filter-values={1}");
            var sortByQuery = BuildQueryFromJson(sortBy, "sortby={0}&order={1}");
            var limitQuery = BuildQueryFromJson(limit,"{0}={1}");
            var offsetQuery = BuildQueryFromJson(offset,"{0}={1}");
            var subfoldersQuery = BuildQueryFromJson(subfolders,"{0}={1}");
            var withTeamDocsQuery = BuildQueryFromJson(withTeamDocs,"{0}={1}");
            var includeDocsSubfolderQuery = BuildQueryFromJson(includeDocsSubfolder,"{0}={1}");

            var options = new List<string>
                    (new []
                    {
                        filterQuery, sortByQuery, limitQuery, offsetQuery,
                        subfoldersQuery, withTeamDocsQuery, includeDocsSubfolderQuery
                    })
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
