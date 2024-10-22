using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;

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
                throw new ArgumentException("Limit must be greater than 0 but less than or equal to 50.");
            }

            if (opts.Offset < 0)
            {
                throw new ArgumentException("Offset must be 0 or greater.");
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
    }
}
