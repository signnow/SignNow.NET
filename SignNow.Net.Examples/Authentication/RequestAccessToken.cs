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
        public static async Task<Token> RequestAccessToken(Uri apiBase, CredentialModel clientInfo, CredentialModel userCredentials)
        {
            Uri apiBaseUrl = apiBase; // "https://api-eval.signnow.com"

            string clientId = clientInfo.Login; // "YOUR_CLIENT_ID";
            string clientSecret = clientInfo.Password; // "YOUR_CLIENT_SECRET";

            string userLogin = userCredentials.Login; // "USER_EMAIL";
            string userPassword = userCredentials.Password; // "USER_PASSWORD";

            var oauth = new OAuth2Service(apiBaseUrl, clientId, clientSecret);

            return await oauth.GetTokenAsync(userLogin, userPassword, Scope.All)
                .ConfigureAwait(false);
        }
    }
}
