using System;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Service
{
    /// <summary>
    /// Folder service allows you to view, create, rename and delete folders for users.
    /// </summary>
    public class FolderService : AuthorizedWebClientBase, IFolderService
    {
        /// <inheritdoc cref="FolderService(Uri, Token, ISignNowClient)"/>
        public FolderService(Token token) : this(ApiUrl.ApiBaseUrl, token, null) { }

        /// <inheritdoc cref="FolderService(Uri, Token, ISignNowClient)"/>
        public FolderService(Uri baseApiUrl, Token token) : this(baseApiUrl, token, null) { }

        /// <summary>
        /// Constructs folder service.
        /// </summary>
        /// <param name="baseApiUrl">Base SignNow API URL.</param>
        /// <param name="token">Access token.</param>
        /// <param name="client">Http client.</param>
        protected internal FolderService(Uri baseApiUrl, Token token, ISignNowClient client)
            : base(baseApiUrl, token, client) { }

        /// <inheritdoc cref="IFolderService.GetAllFoldersAsync"/>
        public async Task<SignNowFolders> GetAllFoldersAsync(CancellationToken cancellationToken = default)
        {
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user/folder"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<SignNowFolders>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IFolderService.GetFolderAsync"/>
        /// <exception cref="System.ArgumentException">If folder identity is not valid.</exception>
        public async Task<SignNowFolders> GetFolderAsync(string folderId, GetFolderOptions options, CancellationToken cancellation = default)
        {
            var query = options?.ToQueryString();
            var filters = string.IsNullOrEmpty(query)
                ? string.Empty
                : $"?{query}";

            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/user/folder/{folderId.ValidateId()}{filters}"),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<SignNowFolders>(requestOptions, cancellation)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IFolderService.CreateFolderAsync"/>
        /// <exception cref="System.ArgumentException">If <paramref name="name"/> is empty.</exception>
        /// <exception cref="System.ArgumentException">If folder <paramref name="parentId"/> is not valid.</exception>
        public async Task<FolderIdentityResponse> CreateFolderAsync(string name, string parentId, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentIsNotEmptyString(name, $"{nameof(name)} cannot be null, empty or whitespace");

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user/folder"),
                Content = new JsonHttpContent(new {name = name, parent_id = parentId.ValidateId()}),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<FolderIdentityResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IFolderService.DeleteFolderAsync"/>
        /// <exception cref="System.ArgumentException">If <paramref name="folderId"/> is not valid.</exception>
        public async Task DeleteFolderAsync(string folderId, CancellationToken cancellationToken = default)
        {
            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/user/folder/{folderId.ValidateId()}"),
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IFolderService.RenameFolderAsync"/>
        /// <exception cref="System.ArgumentException">If <paramref name="name"/> is empty.</exception>
        /// <exception cref="System.ArgumentException">If <paramref name="folderId"/> is not valid.</exception>
        public async Task<FolderIdentityResponse> RenameFolderAsync(string name, string folderId, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(name, $"{nameof(name)} cannot be null, empty or whitespace");

            var requestOptions = new PutHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/user/folder/{folderId.ValidateId()}"),
                Content = new JsonHttpContent(new {name = name}),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<FolderIdentityResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
