using System.IO;

namespace SignNow.Net.Model
{
    public class DownloadDocumentResponse
    {
        /// <summary>
        /// File name with extension.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// File content type.
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// File length in bytes
        /// </summary>
        public long? Length { get; set; }

        /// <summary>
        /// File contents.
        /// </summary>
        public Stream Document { get; set; }
    }
}
