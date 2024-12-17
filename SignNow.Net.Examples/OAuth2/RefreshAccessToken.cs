using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;

namespace SignNow.Net.Examples
{
    public partial class OAuth2Examples
    {
        /// <summary>
        /// Refresh access token example
        /// </summary>
        [TestMethod]
        public async Task RefreshAccessTokenAsync()
        {
            var oauth = new OAuth2Service(ApiBaseUrl, credentials.ClientId, credentials.ClientSecret);

            // Get a valid token
            var validToken = await oauth.GetTokenAsync(credentials.Login, credentials.Password, Scope.All)
                .ConfigureAwait(false);

            // Refresh the token
            var refreshedToken = await oauth.RefreshTokenAsync(validToken)
                .ConfigureAwait(false);

            Assert.AreNotEqual(refreshedToken, validToken);
        }
    }
}
