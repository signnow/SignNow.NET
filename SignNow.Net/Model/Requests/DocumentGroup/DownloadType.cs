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
        Zip,

        /// <summary>
        /// pdf file that contains all the documents of the group
        /// </summary>
        Merged,

        /// <summary>
        /// pdf file with all the documents + attachments, all with the document group stamp, with history.
        /// </summary>
        Certificate,

        /// <summary>
        /// returns zip file with document group that has ID on each page at the documents, attachments and history file in case history has been requested.
        /// </summary>
        Email
    }
}
