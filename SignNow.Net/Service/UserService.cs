using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;

namespace SignNow.Net.Service
{
    public class UserService : AuthorizedWebClientBase, IUserService, ISignInvite
    {
        public UserService(Token token) : this(ApiUrl.ApiBaseUrl, token)
        {
        }

        private UserService(Uri baseApiUrl, Token token) : base(baseApiUrl, token)
        {
        }

        protected internal UserService(Uri baseApiUrl, Token token, ISignNowClient client) : base(baseApiUrl, token, client)
        {
        }

        /// <inheritdoc cref="IUserService.GetCurrentUserAsync" />
        public async Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            var requestOptions = new GetHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user"),
                Token = Token
            };

            return await SignNowClient.RequestAsync<User>(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CreateInviteAsync" />
        /// <exception cref="ArgumentNullException"><see cref="SignInvite"/> cannot be null.</exception>
        public async Task<InviteResponse> CreateInviteAsync(string documentId, SignInvite invite, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(invite, nameof(invite));

            var sender = GetCurrentUserAsync(cancellationToken).Result;
            var inviteContent = JObject.FromObject(invite);
                inviteContent.Add("from", sender.Email);

            var requestFullUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateDocumentId()}/invite");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestFullUrl,
                Content = new JsonHttpContent(inviteContent),
                Token = Token
            };

            return await SignNowClient.RequestAsync<InviteResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CancelInviteAsync(FreeformInvite, CancellationToken)" />
        /// <exception cref="ArgumentNullException"><see cref="FreeformInvite"/> cannot be null.</exception>
        public async Task CancelInviteAsync(FreeformInvite invite, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(invite, nameof(invite));

            await ProcessCancelInviteAsync($"/invite/{invite.Id}/cancel", cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CancelInviteAsync(string, CancellationToken)" />
        /// <exception cref="ArgumentException">Invalid format of Document Id.</exception>
        public async Task CancelInviteAsync(string documentId, CancellationToken cancellationToken = default)
        {
            await ProcessCancelInviteAsync($"/document/{documentId.ValidateDocumentId()}/fieldinvitecancel", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Process Cancel invite request.
        /// </summary>
        /// <param name="requestedDocument">Relative Url to process request.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        private async Task ProcessCancelInviteAsync(string requestedDocument, CancellationToken cancellationToken)
        {
            var requestOptions = new PutHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, requestedDocument),
                Token = Token
            };

            await SignNowClient.RequestAsync(requestOptions, cancellationToken).ConfigureAwait(false);
        }
    }
}
