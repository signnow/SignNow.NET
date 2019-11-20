namespace SignNow.Net.Model
{
    /// <summary>
    /// Possible types for document downloads
    /// </summary>
    public enum DownloadType
    {
        /// <summary>
        /// Raw data of the PDF document that can be written to a blank `.pdf` file.
        /// </summary>
        PdfCollapsed,
        /// <summary>
        /// Raw data of a ZIP file containing the PDF and any attachments
        /// on that document that can be written to blank a `.zip` file.
        /// </summary>
        ZipCollapsed,
        /// <summary>
        /// Raw data of the PDF collapsed document with a table containing the document's history.
        /// </summary>
        PdfWithHistory
    }
}
