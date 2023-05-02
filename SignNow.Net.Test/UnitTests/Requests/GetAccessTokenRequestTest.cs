using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Internal.Requests;

namespace UnitTests.Requests
{
    [TestClass]
    public class GetAccessTokenRequestTest
    {
        [TestMethod]
        public void CreateProperRequestBodyForGrantTypePassword()
        {
            var requestOptions = new GetAccessTokenRequest()
            {
                GrantType = GrantType.Password,
                Username = "login@signnow.com",
                Password = "user_password",
            };

            var expected = @"{
                ""username"": ""login@signnow.com"",
                ""password"": ""user_password"",
                ""grant_type"": ""password"",
                ""scope"": ""*"",
                ""expiration_time"": 60
            }";

            Assert.That.JsonEqual(expected, requestOptions);
        }

        [TestMethod]
        public void CreateProperRequestBodyForGrantTypeRefreshToken()
        {
            var requestOptions = new GetAccessTokenRequest()
            {
                GrantType = GrantType.RefreshToken,
                RefreshToken = "user_refresh_token",
            };

            var expected = @"{
                ""grant_type"": ""refresh_token"",
                ""scope"": ""*"",
                ""refresh_token"": ""user_refresh_token"",
                ""expiration_time"": 60
            }";

            Assert.That.JsonEqual(expected, requestOptions);
        }

        [TestMethod]
        public void CreateProperRequestBodyForGrantTypeAuthorizationCode()
        {
            var requestOptions = new GetAccessTokenRequest()
            {
                GrantType = GrantType.AuthorizationCode,
                AuthorizationCode = "user_authorization_code",
            };

            var expected = @"{
                ""grant_type"": ""authorization_code"",
                ""scope"": ""*"",
                ""code"": ""user_authorization_code"",
                ""expiration_time"": 60
            }";

            Assert.That.JsonEqual(expected, requestOptions);
        }
    }
}
