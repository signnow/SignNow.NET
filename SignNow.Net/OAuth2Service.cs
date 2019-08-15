using Newtonsoft.Json;
using SignNow.Net._Internal.Constants;
using SignNow.Net.Interface;
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
        private string _clientId { get; set; }

        private string _clientSecret { get; set; }

        private Uri _apiBaseUrl { get; set; }

        public OAuth2Service(string clientId, string clientSecret) : this(ApiUrl.ApiBaseUrl, clientId, clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }
        public OAuth2Service(Uri apiBaseUrl, string clientId, string clientSecret) : base(apiBaseUrl)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _apiBaseUrl = apiBaseUrl;
        }
        protected OAuth2Service(Uri apiBaseUrl, string clientId, string clientSecret, ISignNowClient signNowClient) : base(apiBaseUrl, signNowClient)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _apiBaseUrl = apiBaseUrl;
        }
        public async Task<Uri> GetAuthorizationUrlAsync(Scope scope, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default)
        {
            var client = new HttpClient { BaseAddress = _apiBaseUrl };
            var jsonData = new Dictionary<string, string>
                {
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret},
                    { "username", login},
                    { "username", password},
                    { "grant_type", "password"},
                    { "scope", scope.ToString()}
                };
            var content = new FormUrlEncodedContent(jsonData);

            var response = await client.PostAsync(
                "/oauth2/token", content).ConfigureAwait(false);

            var responseString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Token>(responseString);
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
