using System.IO;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from SignNow API for Document download request.
    /// </summary>
    public class DownloadDocumentResponse
    {
        /// <summary>
        /// File name with extension.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// File length in bytes.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// File contents as Stream.
        /// </summary>
        public Stream Document { get; set; }
    }
}
