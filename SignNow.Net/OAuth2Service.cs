using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net
{
    public class OAuth2Service : WebClientBase, IOAuth2Service
    {
        private string ClientId { get; set; }
        private string ClientSecret { get; set; }

        public OAuth2Service(string clientId, string clientSecret) : this(ApiUrl.ApiBaseUrl, clientId, clientSecret)
        {
        }

        public OAuth2Service(Uri apiBaseUrl, string clientId, string clientSecret) : this(apiBaseUrl, clientId, clientSecret, null)
        {
        }

        protected OAuth2Service(Uri apiBaseUrl, string clientId, string clientSecret, ISignNowClient signNowClient) : base(apiBaseUrl, signNowClient)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        ///<inheritdoc/>
        public Uri GetAuthorizationUrl(Uri redirectUrl)
        {
            if (redirectUrl == null)
                throw new ArgumentNullException(nameof(redirectUrl));

            var host = ApiUrl.ApiBaseUrl.Host;
            var targetHost = host;

            if (host.Equals("api-eval.signnow.com", StringComparison.CurrentCultureIgnoreCase))
                targetHost = "eval.signnow.com";
            else if (host.Equals("api.signnow.com", StringComparison.CurrentCultureIgnoreCase))
                targetHost = "signnow.com";

            var hostUri = new Uri($"{ApiUrl.ApiBaseUrl.Scheme}://{targetHost}");
            return new Uri(
                hostUri, relativeUri: $"proxy/index.php/authorize?client_id={WebUtility.UrlEncode(ClientId)}&response_type=code&redirect_uri={WebUtility.UrlEncode(redirectUrl.ToString())}");


        }

        /// <inheritdoc/>
        public async Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, string>
            {
               { "grant_type", "password" },
               { "username", login },
               { "password", password },
               { "scope", scope.AsString() }
            };

            return await ExecuteTokenRequest(body).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<Token> GetTokenAsync(string code, Scope scope, CancellationToken cancellationToken = default)
        {
            var body = new Dictionary<string, string>
            {
               { "grant_type", "authorization_code" },
               { "code", code },
               { "scope", scope.AsString() }
            };

            return await ExecuteTokenRequest(body).ConfigureAwait(false);
        }

        public async Task<Token> RefreshTokenAsync(Token token, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        //public async Task<bool> ValidateTokenAsync(Token token, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        async Task<Token> ExecuteTokenRequest(Dictionary<string, string> body)
        {
            var url = new Uri(ApiBaseUrl, "oauth2/token");

            var plainTextBytes = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            var options = new PostHttpRequestOptions()
            {
                Token = new Token { AccessToken = appToken, TokenType = TokenType.Basic },
                Content = new FormUrlEncodedHttpContent(body),
                RequestUrl = url
            };

            return await SignNowClient.RequestAsync<Token>(options).ConfigureAwait(false);
        }
    }
}