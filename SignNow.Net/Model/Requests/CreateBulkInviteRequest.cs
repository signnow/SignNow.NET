using System;
using System.IO;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Internal.Helpers.Converters;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Request parameters for creating bulk invites from a template.
    /// </summary>
    public class CreateBulkInviteRequest
    {
        /// <summary>
        /// CSV file stream containing invite data.
        /// </summary>
        public Stream CsvFileStream { get; }

        /// <summary>
        /// Name of the CSV file.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Folder where the documents will be created.
        /// </summary>
        public BaseFolder Folder { get; }

        /// <summary>
        /// Subject line for the email invitation.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Custom message to include in the email invitation.
        /// </summary>
        public string EmailMessage { get; set; }

        /// <summary>
        /// Type of signature to be used for the documents.
        /// </summary>
        [JsonProperty("signature_type", NullValueHandling = NullValueHandling.Ignore)]
        public SignatureType? SignatureType { get; set; }

        /// <summary>
        /// Client timestamp for the request (automatically generated).
        /// </summary>
        [JsonProperty("client_timestamp")]
        [JsonConverter(typeof(UnixTimeStampJsonConverter))]
        public DateTime ClientTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the CreateBulkInviteRequest class.
        /// </summary>
        /// <param name="csvFileStream">CSV file stream containing invite data.</param>
        /// <param name="fileName">Name of the CSV file.</param>
        /// <param name="folder">Folder where the documents will be created.</param>
        /// <exception cref="ArgumentNullException">Thrown when any required parameter is null.</exception>
        public CreateBulkInviteRequest(Stream csvFileStream, string fileName, BaseFolder folder)
        {
            CsvFileStream = csvFileStream ?? throw new ArgumentNullException(nameof(csvFileStream));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            Folder = folder ?? throw new ArgumentNullException(nameof(folder));
        }
    }
}
