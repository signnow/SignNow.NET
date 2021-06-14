using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model
{
    public abstract class BaseFolder
    {
        /// <summary>
        /// Unique identifier of the folder.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Identifier for the folder owner.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// The name of the folder.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Timestamp folder was created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime Created { get; set; }

        /// <summary>
        /// Whether or not this folder is shared.
        /// </summary>
        /// /// <remarks>
        /// <c>true</c> - it is a shared folder; <c>false</c> - it's not a shared folder.
        /// </remarks>
        [JsonProperty("shared")]
        public bool Shared { get; set; }
    }

    /// <summary>
    /// Folders serve for storing user's documents.
    /// </summary>
    public class Folder : BaseFolder
    {
        /// <summary>
        /// Amount of documents in this folder.
        /// </summary>
        [JsonProperty("document_count")]
        public int TotalDocuments { get; set; }

        /// <summary>
        /// Amount of templates in this folder.
        /// </summary>
        [JsonProperty("template_count")]
        public int TotalTemplates { get; set; }

        /// <summary>
        /// Amount of subfolders in this folder.
        /// </summary>
        [JsonProperty("folder_count")]
        public int TotalFolders { get; set; }
    }

    /// <summary>
    /// Represents all folders of a user.
    /// </summary>
    public class SignNowFolders : BaseFolder
    {
        /// <summary>
        /// Identifier for the parent folder that contains this folder.
        /// </summary>
        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        /// <summary>
        /// Whether or not this is a system folder.
        /// </summary>
        /// <remarks>
        /// <c>true</c> - it is a system folder; <c>false</c> - it's not a system folder.
        /// </remarks>
        [JsonProperty("system_folder")]
        public bool SystemFolder { get; set; }

        /// <summary>
        /// Amount of documents in this folder.
        /// </summary>
        [JsonProperty("total_documents")]
        public int TotalDocuments { get; set; }

        /// <summary>
        /// <see cref="Folder"/> objects stored in this folder and their attributes.
        /// </summary>
        [JsonProperty("folders")]
        public IReadOnlyCollection<Folder> Folders { get; private set; } = new List<Folder>();

        /// <summary>
        /// <see cref="SignNowDocument"/> objects stored in this folder and their attributes.
        /// </summary>
        [JsonProperty("documents")]
        public IReadOnlyCollection<SignNowDocument> Documents { get; private set; } = new List<SignNowDocument>();
    }
}
