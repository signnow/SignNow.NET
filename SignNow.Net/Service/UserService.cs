using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Service
{
    public class UserService : WebClientBase, IUserService, ISignInvite
    {
        /// <summary>
        /// Constructs User service.
        /// </summary>
        /// <param name="baseApiUrl">Base signNow API URL</param>
        /// <param name="token">Access token</param>
        /// <param name="signNowClient">signNow Http client</param>
        public UserService(Uri baseApiUrl, Token token, ISignNowClient signNowClient = null)
            : base(baseApiUrl, token, signNowClient)
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
                Content = createUser,
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
                Content = updateUser,
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

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user/verifyemail"),
                Content = new SendVerificationEmailRequest { Email = email.ValidateEmail() },
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

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, "/user/forgotpassword"),
                Content = new SendVerificationEmailRequest { Email = email.ValidateEmail() },
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
            invite.From = sender.Email;

            var requestOptions = new PostHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/document/{documentId.ValidateId()}/invite"),
                Content = invite,
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
            var requestOptions = new DeleteHttpRequestOptions
            {
                RequestUrl = new Uri(ApiBaseUrl, $"/v2/documents/{documentId.ValidateId()}/embedded-invites"),
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

            await ProcessCancelInviteAsync($"/invite/{invite.Id}/cancel", cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignInvite.CancelInviteAsync(string, CancellationToken)" />
        /// <exception cref="ArgumentException">Invalid format of <paramref name="documentId"/>.</exception>
        public async Task CancelInviteAsync(string documentId, CancellationToken cancellationToken = default)
        {
            await ProcessCancelInviteAsync($"/document/{documentId.ValidateId()}/fieldinvitecancel", cancellationToken)
                .ConfigureAwait(false);
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

        /// <inheritdoc cref="IUserService.GetModifiedDocumentsAsync" />
        public async Task<IEnumerable<SignNowDocument>> GetModifiedDocumentsAsync(int perPage = 15, CancellationToken cancellationToken = default)
        {
            const string RelativeUrl = "/user/documentsv2";

            return await GetDocumentsAsync(RelativeUrl, perPage, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc cref="IUserService.GetUserDocumentsAsync" />
        public async Task<IEnumerable<SignNowDocument>> GetUserDocumentsAsync(int perPage = 15, CancellationToken cancellationToken = default)
        {
            const string RelativeUrl = "/user/documents";

            return await GetDocumentsAsync(RelativeUrl, perPage, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Returns an enumerable of user's documents.
        /// </summary>
        /// <param name="relativeUrl">Relative URL for documents (modified or not).</param>
        /// <param name="perPage">How many document objects to display per page in response.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns></returns>
        private async Task<IEnumerable<SignNowDocument>> GetDocumentsAsync(string relativeUrl, int perPage = 15, CancellationToken cancellationToken = default)
        {
            bool hasMorePages = true;
            int page = 1;

            var documentsResponse = new List<SignNowDocument>();

            while (hasMorePages)
            {
                var requestOptions = new GetHttpRequestOptions
                {
                    RequestUrl = new Uri(ApiBaseUrl, $"{relativeUrl}?per_page={perPage}&page={page}"),
                    Token = Token
                };

                try
                {
                    var documents = await SignNowClient
                        .RequestAsync<IList<SignNowDocument>>(requestOptions, cancellationToken)
                        .ConfigureAwait(false);

                    documentsResponse.AddRange(documents);

                    if (documents.Count < perPage)
                    {
                        hasMorePages = false;
                    }

                    page += 1;
                }
                catch (SignNowException ex) when (ex.HttpStatusCode == HttpStatusCode.BadRequest)
                {
                    hasMorePages = false;

                    if (documentsResponse.Count == 0)
                        throw;
                }
            }

            return documentsResponse;
        }
    }
}
