using SignNow.Net.Internal.Constants;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Service;
using System;
using System.Net.Http;
using SignNow.Net.Internal.Service;

namespace SignNow.Net
{
    /// <summary>
    /// Service container with all signNow services
    /// </summary>
    public class SignNowContext : WebClientBase, ISignNowContext
    {
        /// <inheritdoc cref="IUserService"/>
        public IUserService Users { get; protected set; }

        /// <inheritdoc cref="ISignInvite"/>
        public ISignInvite Invites { get; protected set; }

        /// <inheritdoc cref="IDocumentService"/>
        public IDocumentService Documents { get; protected set; }

        /// <inheritdoc cref="IFolderService"/>
        public IFolderService Folders { get; protected set; }

        /// <summary>
        /// Create all the services using single instance of <see cref="ISignNowClient"/> and other dependencies.
        /// </summary>
        /// <param name="token">User Access token</param>
        public SignNowContext(Token token) : this(ApiUrl.ApiBaseUrl, token)
        {
        }

        /// <param name="baseApiUrl">Base signNow api URL</param>
        /// <inheritdoc cref="SignNowContext(Token)"/>
        public SignNowContext(Uri baseApiUrl, Token token) : this(baseApiUrl, token, signNowClient: null)
        {
        }

        /// <summary>Create all the services with user provided Http Client</summary>
        /// <inheritdoc cref="SignNowContext(Uri, Token)"/>
        /// <param name="client">User provided Http Client</param>
        public SignNowContext(Uri baseApiUrl, Token token, HttpClient client)
            : this(baseApiUrl, token, new SignNowClient(client))
        {
        }

        /// <inheritdoc cref="SignNowContext(Uri, Token)"/>
        /// <param name="signNowClient">signNow HTTP Client</param>
        private SignNowContext(Uri baseApiUrl, Token token, ISignNowClient signNowClient)
            : base(baseApiUrl, token, signNowClient)
        {
            Users = new UserService(ApiBaseUrl, Token, SignNowClient);
            Invites = (ISignInvite) Users;
            Documents = new DocumentService(ApiBaseUrl, Token, SignNowClient);
            Folders = new FolderService(ApiBaseUrl, Token, SignNowClient);
        }
    }
}
