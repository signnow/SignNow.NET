using System;
using System.IO;
using System.Net.Http;
using System.Text;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// A container for bulk invite template request using multipart/form-data MIME type.
    /// </summary>
    internal class BulkInviteTemplateRequest : IContent
    {
        private readonly CreateBulkInviteRequest _request;

        public BulkInviteTemplateRequest(CreateBulkInviteRequest request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public HttpContent GetHttpContent()
        {
            var content = new MultipartFormDataContent();
            
            // Add CSV file with proper content type
            var csvContent = new StreamContent(_request.CsvFileStream);
            csvContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");
            content.Add(csvContent, "file", _request.FileName);
            
            // Add required folder_id from the folder object
            content.Add(new StringContent(_request.Folder.Id, Encoding.UTF8), "folder_id");
            
            // Add optional parameters if provided
            if (!string.IsNullOrEmpty(_request.Subject))
            {
                content.Add(new StringContent(_request.Subject, Encoding.UTF8), "subject");
            }
            
            if (!string.IsNullOrEmpty(_request.EmailMessage))
            {
                content.Add(new StringContent(_request.EmailMessage, Encoding.UTF8), "email_message");
            }
            
            // Always add client_timestamp as it's automatically generated
            var unixTimestamp = ((DateTimeOffset)_request.ClientTime).ToUnixTimeSeconds();
            content.Add(new StringContent(unixTimestamp.ToString(), Encoding.UTF8), "client_timestamp");
            
            if (_request.SignatureType.HasValue)
            {
                var signatureTypeValue = _request.SignatureType.Value switch
                {
                    SignatureType.Eideasy => "eideasy",
                    SignatureType.EideasyPdf => "eideasy-pdf",
                    SignatureType.Nom151 => "nom151",
                    _ => throw new ArgumentException($"Unknown signature type: {_request.SignatureType.Value}")
                };
                content.Add(new StringContent(signatureTypeValue, Encoding.UTF8), "signature_type");
            }

            return content;
        }
    }
}