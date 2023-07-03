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
using SignNow.Net.Model.EditFields;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Service
{
    public class DocumentService : WebClientBase, IDocumentService
    {
        /// <summary>
        /// Creates new instance of <see cref="DocumentService"/>
        /// </summary>
        /// <param name="baseApiUrl">Base signNow API URL</param>
        /// <param name="token">Access token</param>
        /// <param name="signNowClient">signNow Http client</param>
        public DocumentService(Uri baseApiUrl, Token token, ISignNowClient signNowClient = null)
            : base(baseApiUrl, token, signNowClient)
        {
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <see paramref="documentId"/> is not valid.</exception>
        public async Task<SignNowDocument> GetDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<SignNowDocument>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <see paramref="documentId"/> is not valid.</exception>
        public async Task<SigningLinkResponse> CreateSigningLinkAsync(string documentId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/link"),
                Content = new CreateSigningLinkRequest{ DocumentId = documentId.ValidateId() },
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<SigningLinkResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <see paramref="documentId"/> is not valid.</exception>
        public async Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}"),
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<UploadDocumentResponse> UploadDocumentAsync(Stream documentContent, string fileName, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document", documentContent, fileName, null, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<UploadDocumentResponse> UploadDocumentWithFieldExtractAsync(Stream documentContent, string fileName, ITextTags tags = null, CancellationToken cancellationToken = default)
        {
            return await UploadDocumentAsync("/document/fieldextract", documentContent, fileName, tags, cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task<UploadDocumentResponse> UploadDocumentAsync(string requestRelativeUrl, Stream documentContent, string fileName, ITextTags tags, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, requestRelativeUrl),
                Content = new MultipartHttpContent(documentContent, fileName, tags),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<UploadDocumentResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <see paramref="documentId"/> is not valid.</exception>
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

            Token.TokenType = TokenType.Bearer;
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}/download{query}"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync(requestOptions, new HttpContentToDownloadDocumentResponseAdapter(), HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentNullException">If <see paramref="documents"/> is null.</exception>
        public async Task<DownloadDocumentResponse> MergeDocumentsAsync(string documentName, IEnumerable<SignNowDocument> documents, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(documents, nameof(documents));

            var requestBody = new MergeDocumentRequest { Name = documentName };
            requestBody.AddDocuments(documents);

            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/document/merge"),
                Content = requestBody,
                Token = Token
            };

            var mergeResponse = await SignNowClient
                .RequestAsync(requestOptions, new HttpContentToDownloadDocumentResponseAdapter(), HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            mergeResponse.Filename = documentName;

            return mergeResponse;
        }

        /// <inheritdoc cref="IDocumentService.MoveDocumentAsync"/>
        /// <exception cref="System.ArgumentException">If <paramref name="documentId"/> is not valid.</exception>
        /// <exception cref="System.ArgumentException">If <paramref name="folderId"/> is not valid.</exception>
        public async Task MoveDocumentAsync(string documentId, string folderId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}/move"),
                Content = new MoveDocumentRequest { FolderId = folderId.ValidateId() },
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IDocumentService.GetDocumentHistoryAsync"/>
        /// <exception cref="System.ArgumentException">If <paramref name="documentId"/> is not valid.</exception>
        public async Task<IReadOnlyList<DocumentHistoryResponse>> GetDocumentHistoryAsync(string documentId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}/historyfull"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<List<DocumentHistoryResponse>>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <paramref name="documentId"/> is not valid.</exception>
        public async Task<DownloadLinkResponse> CreateOneTimeDownloadLinkAsync(string documentId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}/download/link"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<DownloadLinkResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <paramref name="documentId"/> is not valid.</exception>
        /// <exception cref="System.ArgumentNullException">If <paramref name="templateName"/> is null.</exception>
        public async Task<CreateTemplateFromDocumentResponse> CreateTemplateFromDocumentAsync(string documentId, string templateName, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(templateName, nameof(templateName));

            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/template"),
                Content = new CreateTemplateFromDocumentRequest(templateName, documentId.ValidateId()),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<CreateTemplateFromDocumentResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <paramref name="templateId"/> is not valid.</exception>
        /// <exception cref="System.ArgumentNullException">If <paramref name="documentName"/> is null.</exception>
        public async Task<CreateDocumentFromTemplateResponse> CreateDocumentFromTemplateAsync(string templateId, string documentName, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(documentName, nameof(documentName));

            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/template/{templateId.ValidateId()}/copy"),
                Content = new CreateDocumentFromTemplateRequest(documentName),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<CreateDocumentFromTemplateResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <see paramref="documentId"/> is not valid.</exception>
        /// <exception cref="System.ArgumentNullException">If <see paramref="fields"/> is null.</exception>
        public async Task PrefillTextFieldsAsync(string documentId, IEnumerable<TextField> fields, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(fields, nameof(fields));

            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PutHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/documents/{documentId.ValidateId()}/prefill-texts"),
                Content = new PrefillTextFieldRequest(fields),
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If <see paramref="documentId"/> is not valid.</exception>
        /// <exception cref="System.ArgumentNullException">If <see paramref="fields"/> is null.</exception>
        public async Task<EditDocumentResponse> EditDocumentAsync(string documentId, IEnumerable<IFieldEditable> fields, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(fields, nameof(fields));

            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PutHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}"),
                Content = new EditFieldRequest(fields),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<EditDocumentResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
