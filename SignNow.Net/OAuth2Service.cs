using Newtonsoft.Json;
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
            var url = $"{ApiBaseUrl}/oauth2/token";
            url = "https://api-eval.signnow.com/oauth2/token";

            var body = new Dictionary<string, string>
            {
               { "grant_type", "password" },
               { "username", login },
               { "password", password }
            };

            var plainTextBytes = Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);

            var options = new PostHttpRequesOptions()
            {
                Token = new Token { AccessToken = appToken, TokenType = TokenTypeEnum.Basic },
                Content = new FormUrlEncodedContent(body),
                RequestUrl = new Uri(url)
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
