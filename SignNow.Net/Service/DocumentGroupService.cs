using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Model.Responses;
using PublicUpdateDocumentGroupTemplateRequest = SignNow.Net.Model.Requests.DocumentGroup.UpdateDocumentGroupTemplateRequest;

namespace SignNow.Net.Service
{
    public class DocumentGroupService : WebClientBase, IDocumentGroup
    {
        /// <summary>
        /// Creates new instance of <see cref="DocumentService"/>
        /// </summary>
        /// <param name="apiBaseUrl">Base signNow API URL</param>
        /// <param name="token">Access token</param>
        /// <param name="signNowClient">signNow Http client</param>
        public DocumentGroupService(Uri apiBaseUrl, Token token, ISignNowClient signNowClient) : base(apiBaseUrl, token, signNowClient)
        {
        }

        /// <inheritdoc />
        public async Task<DocumentGroupCreateResponse> CreateDocumentGroupAsync(string groupName, IEnumerable<SignNowDocument> documents, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/documentgroup"),
                Content = new CreateDocumentGroupRequest(documents) {GroupName = groupName},
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<DocumentGroupCreateResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If document group identity is not valid.</exception>
        public async Task<DocumentGroupInfoResponse> GetDocumentGroupInfoAsync(string documentGroupId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;
            var requestOption = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/document-groups/{documentGroupId.ValidateId()}"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<DocumentGroupInfoResponse>(requestOption, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentException">Limit must be greater than 0 but less than or equal to 50.</exception>
        /// <exception cref="ArgumentException">Offset must be 0 or greater.</exception>
        public async Task<DocumentGroupsResponse> GetDocumentGroupsAsync(IQueryToString options, CancellationToken cancellationToken = default)
        {
            if (options.GetType() != typeof(LimitOffsetOptions))
            {
                throw new ArgumentException("Query params does not have 'limit' and 'offset' options. Use \"LimitOffsetOptions\" class.", nameof(options));
            }

            var opts = (LimitOffsetOptions)options;
            if (opts.Limit <= 0 || opts.Limit > 50)
            {
                throw new ArgumentException("Limit must be greater than 0 but less than or equal to 50.", nameof(options));
            }

            if (opts.Offset < 0)
            {
                throw new ArgumentException("Offset must be 0 or greater.", nameof(options));
            }

            var query = options?.ToQueryString();
            var filters = string.IsNullOrEmpty(query)
                ? string.Empty
                : $"?{query}";

            Token.TokenType = TokenType.Bearer;
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/user/documentgroups{filters}"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<DocumentGroupsResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If document group identity is not valid.</exception>
        public async Task RenameDocumentGroupAsync(string newName, string documentGroupId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;

            var requestOptions = new PutHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/document-groups/{documentGroupId.ValidateId()}"),
                Content = new RenameDocumentGroupRequest { GroupName = newName},
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If document group identity or folder identity is not valid.</exception>
        public async Task MoveDocumentGroupAsync(string documentGroupId, string folderId, bool withSharedDocuments = false, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/document-groups/{documentGroupId.ValidateId()}/move"),
                Content = new MoveDocumentGroupRequest { FolderId = folderId.ValidateId(), WithSharedDocuments = withSharedDocuments },
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If document group identity is not valid.</exception>
        public async Task CopyDocumentGroupAsync(string documentGroupId, string newName, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/document-groups/{documentGroupId.ValidateId()}/copy"),
                Content = new CopyDocumentGroupRequest { DocumentGroupName = newName },
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If document group identity is not valid.</exception>
        public async Task DeleteDocumentGroupAsync(string documentGroupId, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;

            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/documentgroup/{documentGroupId.ValidateId()}"),
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If document group identity is not valid.</exception>
        public async Task<DownloadDocumentResponse> DownloadDocumentGroupAsync(string documentGroupId, DownloadOptions options, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/documentgroup/{documentGroupId.ValidateId()}/downloadall"),
                Content = options,
                Token = Token
            };

            return await SignNowClient
                .RequestAsync(requestOptions, new HttpContentToDownloadDocumentResponseAdapter(), HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If document group template identity is not valid.</exception>
        public async Task<SuccessStatusResponse> UpdateDocumentGroupTemplateAsync(string documentGroupTemplateId, PublicUpdateDocumentGroupTemplateRequest updateRequest, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;

            var requestOptions = new PatchHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/document-group-templates/{documentGroupTemplateId.ValidateId()}"),
                Content = new Internal.Requests.UpdateDocumentGroupTemplateRequest(updateRequest),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<SuccessStatusResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If document group identity is not valid.</exception>
        public async Task CreateDocumentGroupTemplateAsync(string documentGroupId, CreateDocumentGroupTemplateRequest createRequest, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/document-groups/{documentGroupId.ValidateId()}/document-group-template"),
                Content = createRequest,
                Token = Token
            };

            // The API returns 202 Accepted with empty body, so we don't expect a response
            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        /// <exception cref="System.ArgumentException">If request parameters are not valid.</exception>
        public async Task<GetDocumentGroupTemplatesResponse> GetDocumentGroupTemplatesAsync(GetDocumentGroupTemplatesRequest request, CancellationToken cancellationToken = default)
        {
            Token.TokenType = TokenType.Bearer;

            var query = request?.ToQueryString();
            var queryString = string.IsNullOrEmpty(query)
                ? string.Empty
                : $"?{query}";

            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/user/documentgroup/templates{queryString}"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<GetDocumentGroupTemplatesResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
