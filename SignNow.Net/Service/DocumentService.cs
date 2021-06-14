using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Internal.Helpers;
using System.Net.Http;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Service
{
    public class DocumentService : AuthorizedWebClientBase, IDocumentService
    {
        /// <inheritdoc cref="DocumentService(Uri, Token, ISignNowClient)"/>
        public DocumentService(Token token) : this(ApiUrl.ApiBaseUrl, token)
        {
        }

        /// <inheritdoc cref="DocumentService(Uri, Token, ISignNowClient)"/>
        public DocumentService(Uri baseApiUrl, Token token) : this(baseApiUrl, token, null)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="DocumentService"/>
        /// </summary>
        /// <param name="baseApiUrl"><see cref="ApiUrl.ApiBaseUrl"/></param>
        /// <param name="token"><see cref="Token"/></param>
        /// <param name="signNowClient"><see cref="ISignNowClient"/></param>
        protected internal DocumentService(Uri baseApiUrl, Token token, ISignNowClient signNowClient) : base(baseApiUrl, token, signNowClient)
        {
        }

        /// <inheritdoc />
        public async Task<SignNowDocument> GetDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}");
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<SignNowDocument>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestFullUrl = new Uri(ApiBaseUrl, "/link");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestFullUrl,
                Content = new JsonHttpContent(new { document_id = documentId.ValidateId() }),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<SigningLinkResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestedDocument = $"/document/{documentId.ValidateId()}";

            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, requestedDocument),
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document", documentContent, fileName, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<UploadDocumentResponse> UploadDocumentWithFieldExtractAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document/fieldextract", documentContent, fileName, cancellationToken)
                .ConfigureAwait(false);
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

            return await SignNowClient
                .RequestAsync<UploadDocumentResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
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

            var requestedDocument = $"/document/{documentId.ValidateId()}/download{query}";

            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, requestedDocument),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync(requestOptions, new HttpContentToDownloadDocumentResponseAdapter(), HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<DownloadDocumentResponse> MergeDocumentsAsync(string documentName, IEnumerable<SignNowDocument> documents, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(documents, nameof(documents));

            var requestBody = new MergeDocumentRequest
            {
                Name = documentName
            };
            requestBody.AddDocuments(documents);

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/document/merge"),
                Content = new JsonHttpContent(requestBody),
                Token = Token
            };

            var mergeResponse = await SignNowClient
                .RequestAsync(requestOptions, new HttpContentToDownloadDocumentResponseAdapter(), HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            mergeResponse.Filename = documentName;

            return mergeResponse;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<DocumentHistoryResponse>>
            GetDocumentHistoryAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}/historyfull");
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<List<DocumentHistoryResponse>>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<DownloadLinkResponse> CreateOneTimeDownloadLinkAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}/download/link");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<DownloadLinkResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CreateTemplateFromDocumentResponse> CreateTemplateFromDocumentAsync(string documentId, string templateName, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(templateName, nameof(templateName));

            var requestUrl = new Uri(ApiBaseUrl, $"/template");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = Token,
                Content = new JsonHttpContent(new CreateTemplateFromDocumentRequest(templateName, documentId.ValidateId())),
            };

            return await SignNowClient
                .RequestAsync<CreateTemplateFromDocumentResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<CreateDocumentFromTemplateResponse> CreateDocumentFromTemplateAsync(string templateId, string documentName, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(documentName, nameof(documentName));

            var requestUrl = new Uri(ApiBaseUrl, $"/template/{templateId.ValidateId()}/copy");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = Token,
                Content = new JsonHttpContent(new CreateDocumentFromTemplateRequest(documentName))
            };

            return await SignNowClient
                .RequestAsync<CreateDocumentFromTemplateResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
