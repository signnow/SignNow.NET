using System.IO;
using System.Net.Http;
using System.Text;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// A container for bulk invite template request using multipart/form-data MIME type.
    /// </summary>
    internal class BulkInviteTemplateRequest : IContent
    {
        private readonly Stream _csvFileStream;
        private readonly string _fileName;
        private readonly string _folderId;
        private readonly string _subject;
        private readonly string _emailMessage;
        private readonly int? _clientTimestamp;
        private readonly string _signatureType;

        public BulkInviteTemplateRequest(
            Stream csvFileStream, 
            string fileName, 
            string folderId,
            string subject = null,
            string emailMessage = null,
            int? clientTimestamp = null,
            string signatureType = null)
        {
            _csvFileStream = csvFileStream;
            _fileName = fileName;
            _folderId = folderId;
            _subject = subject;
            _emailMessage = emailMessage;
            _clientTimestamp = clientTimestamp;
            _signatureType = signatureType;
        }

        public HttpContent GetHttpContent()
        {
            var content = new MultipartFormDataContent();
            
            // Add CSV file with proper content type
            var csvContent = new StreamContent(_csvFileStream);
            csvContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
            content.Add(csvContent, "file", _fileName);
            
            // Add required folder_id
            content.Add(new StringContent(_folderId, Encoding.UTF8), "folder_id");
            
            // Add optional parameters if provided
            if (!string.IsNullOrEmpty(_subject))
            {
                content.Add(new StringContent(_subject, Encoding.UTF8), "subject");
            }
            
            if (!string.IsNullOrEmpty(_emailMessage))
            {
                content.Add(new StringContent(_emailMessage, Encoding.UTF8), "email_message");
            }
            
            if (_clientTimestamp.HasValue)
            {
                content.Add(new StringContent(_clientTimestamp.Value.ToString(), Encoding.UTF8), "client_timestamp");
            }
            
            if (!string.IsNullOrEmpty(_signatureType))
            {
                content.Add(new StringContent(_signatureType, Encoding.UTF8), "signature_type");
            }

            return content;
        }
    }
}