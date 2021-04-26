using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

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

        /// <inheritdoc cref="IUserService.CreateUserAsync" />
        public async Task<UserCreateResponse> CreateUserAsync(CreateUserOptions createUser, CancellationToken cancellation = default)
        {
            var basicToken = Token;
            basicToken.TokenType = TokenType.Basic;

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user"),
                Content = new JsonHttpContent(createUser),
                Token = basicToken
            };

            return await SignNowClient.RequestAsync<UserCreateResponse>(requestOptions, cancellation)
                .ConfigureAwait(false);
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

        /// <inheritdoc cref="IUserService.UpdateUserAsync" />
        public async Task<UserUpdateResponse> UpdateUserAsync(UpdateUserOptions updateUser, CancellationToken cancellationToken = default)
        {
            var requestOptions = new PutHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user"),
                Content = new JsonHttpContent(updateUser),
                Token = Token
            };

            return await SignNowClient
                .RequestAsync<UserUpdateResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IUserService.SendVerificationEmailAsync" />
        /// <exception cref="ArgumentException"><paramref name="email"/> address is not valid</exception>
        public async Task SendVerificationEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var basicToken = Token;
            basicToken.TokenType = TokenType.Basic;

            var requestBody = new {email = email.ValidateEmail()};

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user/verifyemail"),
                Content = new JsonHttpContent(requestBody),
                Token = basicToken
            };

            await SignNowClient.RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IUserService.SendPasswordResetLinkAsync"/>
        /// <exception cref="ArgumentException"><paramref name="email"/> address is not valid</exception>
        public async Task SendPasswordResetLinkAsync(string email, CancellationToken cancellationToken = default)
        {
            var basicToken = Token;
            basicToken.TokenType = TokenType.Basic;

            var requestBody = new {email = email.ValidateEmail()};

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user/forgotpassword"),
                Content = new JsonHttpContent(requestBody),
                Token = basicToken
            };

            await SignNowClient.RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CreateInviteAsync(string, SignInvite, CancellationToken)" />
        /// <exception cref="ArgumentNullException"><paramref name="invite"/> cannot be null.</exception>
        /// <exception cref="ArgumentException">Invalid format of <paramref name="documentId"/></exception>
        public async Task<InviteResponse> CreateInviteAsync(string documentId, SignInvite invite, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(invite, nameof(invite));

            var sender = GetCurrentUserAsync(cancellationToken).Result;
            var inviteContent = JObject.FromObject(invite);
                inviteContent.Add("from", sender.Email);

            var requestFullUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}/invite");
            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestFullUrl,
                Content = new JsonHttpContent(inviteContent),
                Token = Token
            };

            return await SignNowClient.RequestAsync<InviteResponse>(requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CreateInviteAsync(string, EmbeddedSigningInvite, CancellationToken)" />
        /// <exception cref="ArgumentNullException"><paramref name="invite"/> cannot be null.</exception>
        public async Task<EmbeddedInviteResponse> CreateInviteAsync(string documentId, EmbeddedSigningInvite invite, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(invite, nameof(invite));

            var requestUrl = new Uri(ApiBaseUrl, $"/v2/documents/{documentId.ValidateId()}/embedded-invites");

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = Token,
                Content = new EmbeddedSigningRequest(invite)
            };

            return await SignNowClient
                .RequestAsync<EmbeddedInviteResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.GenerateEmbeddedInviteLinkAsync" />
        /// <exception cref="ArgumentNullException"><paramref name="options"/> cannot be null.</exception>
        public async Task<EmbeddedInviteLinkResponse> GenerateEmbeddedInviteLinkAsync(string documentId, CreateEmbedLinkOptions options, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(options, nameof(options));

            var requestUrl = new Uri(ApiBaseUrl, $"/v2/documents/{documentId.ValidateId()}/embedded-invites/{options.FieldInvite.Id}/link");

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = Token,
                Content = new EmbeddedSigningLinkRequest(options)
            };

            return await SignNowClient
                .RequestAsync<EmbeddedInviteLinkResponse>(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CancelEmbeddedInviteAsync" />
        public async Task CancelEmbeddedInviteAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var requestUrl = new Uri(ApiBaseUrl, $"/v2/documents/{documentId.ValidateId()}/embedded-invites");

            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = requestUrl,
                Token = Token
            };

            await SignNowClient
                .RequestAsync(requestOptions, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CancelInviteAsync(FreeformInvite, CancellationToken)" />
        /// <exception cref="ArgumentNullException"><paramref name="invite"/> cannot be null.</exception>
        public async Task CancelInviteAsync(FreeformInvite invite, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(invite, nameof(invite));

            await ProcessCancelInviteAsync($"/invite/{invite.Id}/cancel", cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CancelInviteAsync(string, CancellationToken)" />
        /// <exception cref="ArgumentException">Invalid format of <paramref name="documentId"/>.</exception>
        public async Task CancelInviteAsync(string documentId, CancellationToken cancellationToken = default)
        {
            await ProcessCancelInviteAsync($"/document/{documentId.ValidateId()}/fieldinvitecancel", cancellationToken).ConfigureAwait(false);
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
