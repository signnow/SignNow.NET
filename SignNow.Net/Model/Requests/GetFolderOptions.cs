using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.Requests
{
    public class GetFolderOptions
    {
        [JsonProperty("filters")]
        public FolderFilters Filters { get; set; }

        /// <summary>
        /// Converts <see cref="FolderFilters"/> to query sting
        /// </summary>
        /// <returns></returns>
        public string ToQueryString()
        {
            var filters = JsonConvert.SerializeObject(Filters);
            var toDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(filters);

            var options = toDictionary
                .Select(k => $"filters={k.Key}&filter-values={k.Value}");

            return string.Join("&", options);
        }
    }

    [JsonObject]
    public sealed class FolderFilters
    {
        /// <summary>
        /// Documents signing status to filter.
        /// </summary>
        [JsonProperty("signing-status", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SigningStatus? Status { get; private set; }

        /// <summary>
        /// Timestamp document was updated.
        /// </summary>
        [JsonProperty("document-updated", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime? Updated { get; private set; }

        /// <summary>
        /// Timestamp document was created.
        /// </summary>
        [JsonProperty("document-created", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime? Created { get; private set; }

        /// <summary>
        /// Construct <see cref="FolderFilters"/> with signing status.
        /// </summary>
        /// <param name="status">Specific status for filtering.</param>
        public FolderFilters(SigningStatus status) => Status = status;

        /// <summary>
        /// Construct <see cref="FolderFilters"/> with document created filter.
        /// </summary>
        /// <param name="documentCreatedFilter">Specific time point to filter documents that were created starting from a specific date.</param>
        public FolderFilters(DocumentCreatedFilter documentCreatedFilter)
        {
            Created = documentCreatedFilter?.Created.ToUniversalTime();
        }

        /// <summary>
        /// Construct <see cref="FolderFilters"/> with document updated filter.
        /// </summary>
        /// <param name="documentUpdatedFilter">Specific time point to filter documents that were updated starting from a specific date.</param>
        public FolderFilters(DocumentUpdatedFilter documentUpdatedFilter)
        {
            Updated = documentUpdatedFilter?.Updated.ToUniversalTime();
        }
    }

    /// <summary>
    /// Filter documents that were created starting from a specific date.
    /// </summary>
    public class DocumentCreatedFilter
    {
        public DateTime Created { get; private set; }

        public DocumentCreatedFilter(DateTime date) => Created = date;
    }

    /// <summary>
    /// Filter documents that were updated starting from a specific date.
    /// </summary>
    public class DocumentUpdatedFilter
    {
        public DateTime Updated { get; private set; }

        public DocumentUpdatedFilter(DateTime date) => Updated = date;
    }

    /// <summary>
    /// Document signing statuses.
    /// </summary>
    public enum SigningStatus
    {
        [EnumMember(Value = "waiting-for-me")]
        WaitingForMe,

        [EnumMember(Value = "waiting-for-others")]
        WaitingForOthers,

        [EnumMember(Value = "signed")]
        Signed,

        [EnumMember(Value = "pending")]
        Pending
    }
}
