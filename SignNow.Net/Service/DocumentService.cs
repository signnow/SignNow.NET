using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Internal.Helpers;
using System.Net.Http;

namespace SignNow.Net.Service
{
    public class DocumentService : AuthorizedWebClientBase, IDocumentService
    {
        public DocumentService(Token token) : this(ApiUrl.ApiBaseUrl, token)
        {
        }

        public DocumentService(Uri baseApiUrl, Token token) : base(baseApiUrl, token)
        {
        }

        protected internal DocumentService(Uri baseApiUrl, Token token, ISignNowClient signNowClient) : base(baseApiUrl, token, signNowClient)
        {
        }

        /// <inheritdoc />
        public async Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestFullUrl = new Uri(ApiBaseUrl, "/link");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestFullUrl,
                Content = new JsonHttpContent(new { document_id = documentId }),
                Token = Token
            };

            return await SignNowClient.RequestAsync<SigningLinkResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestedDocument = "/document/" + documentId.ValidateDocumentId();

            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, requestedDocument),
                Token = Token
            };

            await SignNowClient.RequestAsync(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document", documentContent, fileName, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<UploadDocumentResponse> UploadDocumentWithFieldExtractAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document/fieldextract", documentContent, fileName, cancellationToken).ConfigureAwait(false);
        }

        private async Task<UploadDocumentResponse> UploadDocumentAsync(string requestRelativeUrl, Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            var requestFullUrl = new Uri(ApiBaseUrl, requestRelativeUrl);
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestFullUrl,
                Content = new FileHttpContent(documentContent, fileName),
                Token = Token
            };

            return await SignNowClient.RequestAsync<UploadDocumentResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<DownloadDocumentResponse> DownloadDocumentAsync(string documentId, DownloadType type = DownloadType.PdfCollapsed, CancellationToken cancellationToken = default)
        {
            var query = "";

            switch (type)
            {
                case DownloadType.ZipCollapsed:
                    query = "?type=zip";
                    break;

                case DownloadType.PdfWithHistory:
                    query = "/collapsed?with_history=1";
                    break;

                case DownloadType.PdfOriginal:
                    break;

                case DownloadType.PdfCollapsed:
                default:
                    query = "?type=collapsed";
                    break;
            }

            var requestedDocument = "/document/" + documentId.ValidateDocumentId() + "/download" + query;

            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, requestedDocument),
                Token = Token
            };

            return await SignNowClient.RequestAsync(
                requestOptions,
                new HttpContentToDownloadDocumentResponseAdapter(),
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken
                ).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<InviteResponse> CreateInviteAsync(string documentId, ISignInvite invite, CancellationToken cancellationToken = default)
        {
            if (null == invite)
                throw new ArgumentNullException(nameof(invite));

            var requestFullUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateDocumentId()}/invite");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestFullUrl,
                Content = invite.InviteContent(),
                Token = Token
            };

            return await SignNowClient.RequestAsync<InviteResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}
