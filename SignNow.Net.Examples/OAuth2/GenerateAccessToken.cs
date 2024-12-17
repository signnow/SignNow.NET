using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;

namespace SignNow.Net.Examples
{
    [TestClass]
    public partial class OAuth2Examples : ExamplesRunner
    {
        /// <summary>
        /// An example of obtaining an access token via OAuth 2.0 service.
        /// </summary>
        [TestMethod]
        public async Task GenerateAccessTokenAsync()
        {
            var clientId = credentials.ClientId;
            var clientSecret = credentials.ClientSecret;

            var userLogin = credentials.Login;
            var userPassword = credentials.Password;

            var oauth = new OAuth2Service(ApiBaseUrl, clientId, clientSecret)
            {
                ExpirationTime = 60
            };

            var response = await oauth.GetTokenAsync(userLogin, userPassword, Scope.All)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsFalse(string.IsNullOrEmpty(response.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(response.RefreshToken));
        }
    }
}
