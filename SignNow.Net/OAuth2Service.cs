using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.Requests;

namespace SignNow.Net
{
    /// <summary>
    /// OAuth2 implementation of <see cref="IOAuth2Service"/>
    /// </summary>
    public class OAuth2Service : WebClientBase, IOAuth2Service
    {
        /// <summary>
        /// Application client identity.
        /// </summary>
        private string ClientId { get; set; }

        /// <summary>
        /// Application client secret.
        /// </summary>
        private string ClientSecret { get; set; }

        /// <summary>
        /// SignNow OAuth request <see cref="Uri"/>
        /// </summary>
        private Uri OAuthRequestUrl { get; set; }

        /// <inheritdoc cref="OAuth2Service(Uri, string, string, ISignNowClient)" />
        public OAuth2Service(string clientId, string clientSecret) : this(ApiUrl.ApiBaseUrl, clientId, clientSecret)
        {
        }

        /// <inheritdoc cref="OAuth2Service(Uri, string, string, ISignNowClient)" />
        public OAuth2Service(Uri apiBaseUrl, string clientId, string clientSecret) : this(apiBaseUrl, clientId, clientSecret, null)
        {
        }

        /// <summary>
        /// Constructs an <see cref="OAuth2Service"/>
        /// </summary>
        /// <param name="apiBaseUrl">SignNow API <see cref="WebClientBase.ApiBaseUrl"/></param>
        /// <param name="clientId">Application <see cref="ClientId"/></param>
        /// <param name="clientSecret">Application <see cref="ClientSecret"/></param>
        /// <param name="signNowClient">Http Client</param>
        protected OAuth2Service(Uri apiBaseUrl, string clientId, string clientSecret, ISignNowClient signNowClient) : base(apiBaseUrl, signNowClient)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            OAuthRequestUrl = new Uri(ApiBaseUrl, "oauth2/token");
        }

        /// <inheritdoc cref="IOAuth2Service.GetAuthorizationUrl" />
        public Uri GetAuthorizationUrl(Uri redirectUrl)
        {
            Guard.ArgumentNotNull(redirectUrl, nameof(redirectUrl));

            var targetHost = ApiUrl.ApiBaseUrl.Host;

            if (ApiUrl.ApiBaseUrl.Host.Equals("api-eval.signnow.com", StringComparison.CurrentCultureIgnoreCase))
            {
                targetHost = "eval.signnow.com";
            }
            else if (ApiUrl.ApiBaseUrl.Host.Equals("api.signnow.com", StringComparison.CurrentCultureIgnoreCase))
            {
                targetHost = "signnow.com";
            }

            return new Uri(
                new Uri($"{ApiUrl.ApiBaseUrl.Scheme}://{targetHost}"),
                $"proxy/index.php/authorize?client_id={WebUtility.UrlEncode(ClientId)}&response_type=code&redirect_uri={WebUtility.UrlEncode(redirectUrl.ToString())}");
        }

        /// <inheritdoc cref="IOAuth2Service.GetTokenAsync(string, string, Scope, CancellationToken)" />
        public async Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, string>
            {
               { "grant_type", "password" },
               { "username", login },
               { "password", password },
               { "scope", scope.AsString() }
            };

            return await ExecuteTokenRequest(body, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc cref="IOAuth2Service.GetTokenAsync(string, Scope, CancellationToken)" />
        public async Task<Token> GetTokenAsync(string code, Scope scope, CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, string>
            {
               { "grant_type", "authorization_code" },
               { "code", code },
               { "scope", scope.AsString() }
            };

            var token = await ExecuteTokenRequest(body, cancellationToken).ConfigureAwait(false);

            var tokenLifetime = token.ExpiresIn - (int) UnixTimeStampConverter.ToUnixTimestamp(DateTime.UtcNow);
            if (tokenLifetime > 0) token.ExpiresIn = tokenLifetime ;

            return token;
        }

        /// <inheritdoc cref="IOAuth2Service.RefreshTokenAsync" />
        public async Task<Token> RefreshTokenAsync(Token token, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(token, nameof(token));

            var body = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", token.RefreshToken }
            };

            return await ExecuteTokenRequest(body, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc cref="IOAuth2Service.ValidateTokenAsync" />
        public async Task<bool> ValidateTokenAsync(Token token, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(token, nameof(token));

            var options = new GetHttpRequestOptions
            {
                Token = new Token { AccessToken = token.AccessToken, TokenType = TokenType.Bearer },
                RequestUrl = OAuthRequestUrl
            };

            try
            {
                await SignNowClient.RequestAsync<Token>(options, cancellationToken).ConfigureAwait(false);
            }
            catch (SignNowException ex) when (ex.Message == "invalid_token")
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Processing Http request for Token issue.
        /// </summary>
        /// <param name="body">Dictionary with requested params.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns><see cref="Token"/> response</returns>
        private async Task<Token> ExecuteTokenRequest(Dictionary<string, string> body, CancellationToken cancellationToken = default)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            var options = new PostHttpRequestOptions
            {
                Token = new Token { AppToken = appToken, TokenType = TokenType.Basic },
                Content = new FormUrlEncodedHttpContent(body),
                RequestUrl = OAuthRequestUrl
            };

            var token = await SignNowClient.RequestAsync<Token>(options, cancellationToken).ConfigureAwait(false);
            token.AppToken = appToken;

            return token;
        }
    }
}
