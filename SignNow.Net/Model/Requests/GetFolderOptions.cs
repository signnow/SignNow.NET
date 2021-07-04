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
        private const string FiltersSchema = "filters={0}&filter-values={1}";
        private const string SortSchema = "sortby={0}&order={1}";
        private const string ParamsSchema = "{0}={1}";

        [JsonProperty("filters")]
        public FolderFilters Filters { get; set; }

        [JsonProperty("sortby")]
        public FolderSort SortBy { get; set; }


        [JsonProperty("limit")]
        private Dictionary<string, int> InternalLimit { get; set; }

        [JsonProperty("offset")]
        private Dictionary<string, int> InternalOffset { get; set; }

        [JsonProperty("entity_type")]
        private Dictionary<string, EntityType> InternalEntityType { get; set; }

        [JsonProperty("subfolder-data")]
        private Dictionary<string, SubFolders> InternalSubfolderData { get; set; }

        [JsonProperty("with_team_documents")]
        private Dictionary<string, bool> InternalWithTeamDocument { get; set; }

        [JsonProperty("include_documents_subfolders")]
        private Dictionary<string, bool> InternalIncludeDocumentsSubfolder { get; set; }

        [JsonProperty("exclude_documents_relations")]
        private Dictionary<string, bool> InternalExcludeDocumentsRelations { get; set; }

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
        /// Displays documents by entity type
        /// </summary>
        /// <remarks>
        /// <see cref="EntityType.All"/> - show document list with active documents and document groups.<br/>
        /// <see cref="EntityType.Document"/> - show standard document list.<br/>
        /// <see cref="EntityType.DocumentGroup"/> - show only document groups.
        /// </remarks>
        [JsonIgnore]
        public EntityType EntityTypes
        {
            get => InternalEntityType.Values.FirstOrDefault();
            set => InternalEntityType = new Dictionary<string, EntityType> {{"entity_type", value}};
        }

        /// <summary>
        /// Allows to returns information about all system folders and their sub-folders without documents
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
        /// Allows to display short list of document info and increases maximum limit from 100 to 500 documents per page.
        /// </summary>
        public bool ExcludeDocumentsRelations
        {
            get => InternalExcludeDocumentsRelations.Values.FirstOrDefault();
            set => InternalExcludeDocumentsRelations = new Dictionary<string, bool> {{"exclude_documents_relations", value}};
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
            var entityType = JsonConvert.SerializeObject(InternalEntityType);
            var subfolders = JsonConvert.SerializeObject(InternalSubfolderData);
            var withTeamDocs = JsonConvert.SerializeObject(InternalWithTeamDocument);
            var includeDocsSubfolder = JsonConvert.SerializeObject(InternalIncludeDocumentsSubfolder);
            var excludeDocsRelations = JsonConvert.SerializeObject(InternalExcludeDocumentsRelations);

            var filterQuery = BuildQueryFromJson(filters, FiltersSchema);
            var sortByQuery = BuildQueryFromJson(sortBy, SortSchema);
            var limitQuery = BuildQueryFromJson(limit,ParamsSchema);
            var offsetQuery = BuildQueryFromJson(offset,ParamsSchema);
            var entityTypeQuery = BuildQueryFromJson(entityType,ParamsSchema);
            var subfoldersQuery = BuildQueryFromJson(subfolders,ParamsSchema);
            var withTeamDocsQuery = BuildQueryFromJson(withTeamDocs,ParamsSchema);
            var includeDocsSubfolderQuery = BuildQueryFromJson(includeDocsSubfolder,ParamsSchema);
            var excludeDocsRelationsQuery = BuildQueryFromJson(excludeDocsRelations,ParamsSchema);

            var options = new List<string>
                    (new []
                    {
                        filterQuery, sortByQuery, limitQuery, offsetQuery, entityTypeQuery,
                        subfoldersQuery, withTeamDocsQuery, includeDocsSubfolderQuery, excludeDocsRelationsQuery
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
