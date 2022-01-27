using System;
using System.Threading.Tasks;
using SignNow.Net.Model;
using SignNow.Net.Test.Context;

namespace SignNow.Net.Examples.Authentication
{
    public static class AuthenticationExamples
    {
        /// <summary>
        /// An example of obtaining an access token via OAuth 2.0 service.
        /// </summary>
        /// <param name="apiBase">signNow API base URL. Sandbox: "https://api-eval.signnow.com", Production: "https://api.signnow.com"</param>
        /// <param name="credentials"><see cref="CredentialModel"/> with Application Client ID, Client Secret, Login and Password</param>
        public static async Task<Token> RequestAccessToken(Uri apiBase, CredentialModel credentials)
        {
            Uri apiBaseUrl = apiBase;

            string clientId = credentials.ClientId;
            string clientSecret = credentials.ClientSecret;

            string userLogin = credentials.Login;
            string userPassword = credentials.Password;

            var oauth = new OAuth2Service(apiBaseUrl, clientId, clientSecret);

            return await oauth.GetTokenAsync(userLogin, userPassword, Scope.All)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Verify access token example
        /// </summary>
        /// <param name="apiBaseUrl">signNow API base URL. Sandbox: "https://api-eval.signnow.com", Production: "https://api.signnow.com"</param>
        /// <param name="clientInfo"><see cref="CredentialModel"/> with Application Client ID and Client Secret</param>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task<bool> VerifyAccessToken(Uri apiBaseUrl, CredentialModel clientInfo, Token token)
        {
            var oauth2 = new OAuth2Service(apiBaseUrl, clientInfo.Login, clientInfo.Password);

            return await oauth2.ValidateTokenAsync(token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Refresh access token example
        /// </summary>
        /// <param name="apiBaseUrl">signNow API base URL. Sandbox: "https://api-eval.signnow.com", Production: "https://api.signnow.com"</param>
        /// <param name="clientInfo"><see cref="CredentialModel"/> with Application Client ID and Client Secret</param>
        /// <param name="token">Access token</param>
        /// <returns></returns>
        public static async Task<Token> RefreshAccessToken(Uri apiBaseUrl, CredentialModel clientInfo, Token token)
        {
            var oauth2 = new OAuth2Service(apiBaseUrl, clientInfo.Login, clientInfo.Password);

            return await oauth2.RefreshTokenAsync(token)
                .ConfigureAwait(false);
        }
    }
}
