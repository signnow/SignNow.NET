using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
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
                // Content = new CreateSigningLinkRequest{ DocumentId = documentId.ValidateId() },
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<DocumentGroupCreateResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
