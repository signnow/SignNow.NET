using SignNow.Net._Internal.Requests;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Service;
using System;
using System.Collections.Generic;
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
        public async Task<Uri> GetAuthorizationUrlAsync(string redirectUrl, CancellationToken cancellationToken = default(CancellationToken))
        {
            var uriBuilder = new UriBuilder(ApiUrl.ApiBaseUrl);

            if (ApiUrl.ApiBaseUrl.Host.Equals("api-eval.signnow.com")) //test
                uriBuilder.Host = "eval.signnow.com";
            else if (ApiUrl.ApiBaseUrl.Host.Equals("api.signnow.com")) // prod
                uriBuilder.Host = "signnow.com";

            var relativeUrl = $"proxy/index.php/authorize?client_id={ClientId}&response_type=code&redirect_uri={redirectUrl}";

            return new Uri(uriBuilder.Uri, relativeUrl);
        }

        ///<inheritdoc/>
        public async Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiBaseUrl}oauth2/token";

            var body = new Dictionary<string, string>
            {
               { "grant_type", "password" },
               { "username", login },
               { "password", password },
               { "scope", scope.AsString() }
            };

            var plainTextBytes = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            var options = new PostHttpRequesOptions()
            {
                Token = new Token { AccessToken = appToken, TokenType = TokenType.Basic },
                Content = new FormUrlEncodedHttpContent(body),
                RequestUrl = new Uri(url)
            };

            return await SignNowClient.RequestAsync<Token>(options).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task<Token> GetTokenAsync(string code, Scope scope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"{ApiBaseUrl}oauth2/token";

            var body = new Dictionary<string, string>
            {
               { "grant_type", "authorization_code" },
               { "code", code },
               { "scope", scope.AsString() }
            };

            var plainTextBytes = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);
            var options = new PostHttpRequesOptions()
            {
                Token = new Token { AccessToken = appToken, TokenType = TokenType.Basic },
                Content = new FormUrlEncodedHttpContent(body),
                RequestUrl = new Uri(url)
            };

            return await SignNowClient.RequestAsync<Token>(options).ConfigureAwait(false);
        }

        public Task<Token> RefreshTokenAsync(Token token, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateTokenAsync(Token token, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}