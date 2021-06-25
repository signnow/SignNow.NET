using System;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

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
    }
}
