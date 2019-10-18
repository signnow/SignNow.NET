using SignNow.Net.Internal.Constants;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Internal.Requests;
using System.Text.RegularExpressions;

namespace SignNow.Net.Service
{
    public class DocumentService : AuthorizedWebClientBase, IDocumentService
    {
        /// <summary>
        /// Pattern for Document Identyty
        /// Allowed format must be: 40-chars
        /// Allowed chars: digist, symbols a-z A-Z, underscore
        /// </summary>
        private static string documentIdPattern = @"^[a-zA-Z0-9_]{40,40}$";

        public DocumentService(Token token) : this(ApiUrl.ApiBaseUrl, token)
        {

        }
        public DocumentService(Uri baseApiUrl, Token token) : base(baseApiUrl, token)
        {

        }
        internal protected DocumentService(Uri baseApiUrl, Token token, ISignNowClient signNowClient) : base(baseApiUrl, token, signNowClient)
        {

        }

        ///<inheritdoc/>
        public async Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestFullUrl = new Uri(ApiBaseUrl, "/link");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestFullUrl,
                Content = new JsonHttpContent(new { document_id = documentId }),
                Token = this.Token
            };
            return await SignNowClient.RequestAsync<SigningLinkResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            ValidateDocumentId(documentId);

            var requestedDocument = "/document/" + documentId;

            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, requestedDocument),
                Token = this.Token
            };

            await SignNowClient.RequestAsync(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document", documentContent, fileName, cancellationToken).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task<UploadDocumentResponse> UploadDocumentWithFieldExtractAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document/fieldextract", documentContent, fileName, cancellationToken).ConfigureAwait(false);
        }

        private async Task<UploadDocumentResponse> UploadDocumentAsync (string requestRelativeUrl, Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            var requestFullUrl = new Uri(ApiBaseUrl, requestRelativeUrl);
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestFullUrl,
                Content = new FileHttpContent(documentContent, fileName),
                Token = this.Token
            };
            return await SignNowClient.RequestAsync<UploadDocumentResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates Document ID
        /// </summary>
        /// <param name="documentId">Identity of the document</param>
        private void ValidateDocumentId(string documentId)
        {
            var regex = new Regex(documentIdPattern);

            if (!regex.IsMatch(documentId))
            {
                throw new ArgumentException(
                    "Wrong Document Id <" +
                    documentId +
                    ">. Allowed format should contains chars only [a-zA-Z0-9_] and lenght should be 40 symbols."
                    );
            }
        }
    }
}
