using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;

namespace SignNow.Net.Examples
{
    public partial class OAuth2Examples
    {
        /// <summary>
        /// An example of validating an access token via OAuth 2.0 service.
        /// </summary>
        [TestMethod]
        public async Task VerifyAccessTokenAsync()
        {
            var oauth = new OAuth2Service(ApiBaseUrl, credentials.Login, credentials.Password);

            var dummyToken = new Token
            {
                AccessToken = "dummyAccessToken",
                AppToken = "dummyAppToken",
                ExpiresIn = 100,
                Scope = "*",
                TokenType = TokenType.Bearer
            };

            var dummyTokenResponse = await oauth.ValidateTokenAsync(dummyToken)
                .ConfigureAwait(false);

            Assert.IsFalse(dummyTokenResponse);
        }
    }
}
