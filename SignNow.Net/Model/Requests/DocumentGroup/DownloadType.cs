using System.Runtime.Serialization;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Types of download for document group
    /// </summary>
    public enum DownloadType
    {
        /// <summary>
        /// zipped binary file (application/zip content)
        /// </summary>
        [EnumMember(Value = "zip")]
        Zip,

        /// <summary>
        /// pdf file that contains all the documents of the group
        /// </summary>
        [EnumMember(Value = "merged")]
        Merged,

        /// <summary>
        /// pdf file with all the documents + attachments, all with the document group stamp, with history.
        /// </summary>
        [EnumMember(Value = "certificate")]
        Certificate,

        /// <summary>
        /// returns zip file with document group that has ID on each page at the documents, attachments and history file in case history has been requested.
        /// </summary>
        [EnumMember(Value = "email")]
        Email
    }
}
