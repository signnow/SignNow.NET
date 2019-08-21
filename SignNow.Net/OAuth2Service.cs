using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Model;
using SignNow.Net.Service;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net
{
    public class OAuth2Service : WebClientBase, IOAuth2Service
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        public OAuth2Service(string clientId, string clientSecret) : this(ApiUrl.ApiBaseUrl, clientId, clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
        public OAuth2Service(Uri apiBaseUrl, string clientId, string clientSecret) : base(apiBaseUrl)
        {
        }

        protected OAuth2Service(Uri apiBaseUrl, string clientId, string clientSecret, ISignNowClient signNowClient) : base(apiBaseUrl, signNowClient)
        {
        }

        public async Task<Uri> GetAuthorizationUrlAsync(Scope scope, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default)
        {
            var options = new RequestOptions
            {
                URL = $"{ApiUrl.ApiBaseUrl}oauth2/token",
                GrantType = "password",
                ContentType = "application/x-www-form-urlencoded",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Scope = scope,
                Login = login,
                Password = password
            };

            return await SignNowClient.RequestAsync<Token>(options);
        }

        public async Task<Token> GetTokenAsync(string code, Scope scope, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
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
