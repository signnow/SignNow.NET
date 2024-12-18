using SignNow.Net.Internal.Constants;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Service;
using System;
using System.Net.Http;

namespace SignNow.Net
{
    /// <summary>
    /// Service container with all signNow services
    /// </summary>
    public class SignNowContext : WebClientBase, ISignNowContext
    {
        /// <inheritdoc cref="IOAuth2Service"/>
        public OAuth2Service OAuth { get; protected set; }

        /// <inheritdoc cref="IUserService"/>
        public IUserService Users { get; protected set; }

        /// <inheritdoc cref="ISignInvite"/>
        public ISignInvite Invites { get; protected set; }

        /// <inheritdoc cref="IDocumentService"/>
        public IDocumentService Documents { get; protected set; }

        /// <inheritdoc cref="IFolderService"/>
        public IFolderService Folders { get; protected set; }

        /// <inheritdoc cref="IEventSubscriptionService"/>
        public IEventSubscriptionService Events { get; protected set; }

        /// <inheritdoc cref="IDocumentGroup"/>
        public IDocumentGroup DocumentGroup { get; protected set; }

        /// <summary>
        /// Create all the services using single instance of <see cref="ISignNowClient"/> and other dependencies.
        /// </summary>
        /// <param name="token">User Access token</param>
        public SignNowContext(Token token) : this(ApiUrl.ApiBaseUrl, token)
        {
        }

        /// <summary>Create all the services with user provided Http Client</summary>
        /// <param name="baseApiUrl">Base signNow api URL</param>
        /// <inheritdoc cref="SignNowContext(Token)"/>
        /// <param name="client">User provided Http Client</param>
        public SignNowContext(Uri baseApiUrl, Token token, HttpClient client)
            : this(baseApiUrl, token, new SignNowClient(client))
        {
        }

        /// <param name="baseApiUrl">Base signNow api URL</param>
        /// <inheritdoc cref="SignNowContext(Token)"/>
        /// <param name="signNowClient">signNow HTTP Client</param>
        public SignNowContext(Uri baseApiUrl, Token token, ISignNowClient signNowClient = null)
            : base(baseApiUrl, token, signNowClient)
        {
            OAuth = new OAuth2Service(ApiBaseUrl, "", "", SignNowClient);
            Users = new UserService(ApiBaseUrl, Token, SignNowClient);
            Invites = (ISignInvite) Users;
            Documents = new DocumentService(ApiBaseUrl, Token, SignNowClient);
            Folders = new FolderService(ApiBaseUrl, Token, SignNowClient);
            Events = new EventSubscriptionService(ApiBaseUrl, Token, SignNowClient);
            DocumentGroup = new DocumentGroupService(ApiBaseUrl, Token, SignNowClient);
        }

        /// <summary>
        /// Setup application client ID/Secret for authorization.
        /// </summary>
        /// <param name="clientId">Application client ID</param>
        /// <param name="clientSecret">Application client Secret</param>
        public void SetAppCredentials(string clientId, string clientSecret)
        {
            OAuth.ClientId = clientId;
            OAuth.ClientSecret = clientSecret;
        }

        public Token GetAccessToken(string login, string password, Scope scope)
        {
            Token = OAuth.GetTokenAsync(login, password, scope).Result;
            SyncTokens();
            return Token;
        }

        private void SyncTokens()
        {
            ((UserService)Users).Token = Token;
            ((DocumentService)Documents).Token = Token;
            ((FolderService)Folders).Token = Token;
            ((EventSubscriptionService)Events).Token = Token;
            ((DocumentGroupService)DocumentGroup).Token = Token;
        }
    }
}
