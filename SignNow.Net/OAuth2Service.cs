using SignNow.Net.Internal.Constants;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Service;
using System;
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

        /// <summary>
        /// Get Url of login page
        /// </summary>
        /// <param name="scope">request parameter</param>
        /// <param name="redirectUrl">URL for retireving token</param>
        /// <param name="cancellationToken">cancel operation</param>
        /// <returns></returns>
        public async Task<Uri> GetAuthorizationUrlAsync(Scope scope, string redirectUrl,  CancellationToken cancellationToken = default(CancellationToken))
        {
            return new Uri ($"{ApiUrl.ApiBaseUrl}proxy/index.php/authorize?client_id={ClientId}&response_type=code&redirect_uri={redirectUrl}");
        }

        public async Task<Token> GetTokenAsync(string login, string password, Scope scope, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrive Access Token with Auth. code
        /// </summary>
        /// <param name="code">authorization code</param>
        /// <param name="scope">request parameter</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Token> GetTokenAsync(string code, Scope scope, CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = new RequestOptions
            {
                URL = $"{ApiUrl.ApiBaseUrl}oauth2/token",
                GrantType = "authorization_code",
                AuthorizationCode = code,
                ContentType = "application/x-www-form-urlencoded",
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Scope = scope
            };

            return await SignNowClient.RequestAsync<Token>(options);
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
