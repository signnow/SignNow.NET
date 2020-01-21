using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class OAuth2ServiceTest
    {
        private readonly string RedirectUrl = $"https://github.com/signnow/SignNow.NET";
        private readonly string RelativeAuthUrl = $"/proxy/index.php/authorize";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CannotGetAuthUrlWithoutRedirectUrl()
        {
            var OAuth2 = new OAuth2Service("clientId", "clientSecret");
            OAuth2.GetAuthorizationUrl(null);
        }

        [DataTestMethod]
        [DataRow("api.signnow.com", "signnow.com", DisplayName = "signnow.com")]
        [DataRow("api-eval.signnow.com", "eval.signnow.com", DisplayName = "eval.signnow.com")]
        public void ShouldProperConstructAuthUrl(string testUrl, string expectedHost)
        {
            var apiUrl = new Uri($"https://{testUrl}");
            ApiUrl.ApiBaseUrl = apiUrl;

            var redirect = new Uri(RedirectUrl);
            var expectedUrl = $"https://{expectedHost}{RelativeAuthUrl}?client_id=clientId&response_type=code&redirect_uri={WebUtility.UrlEncode(RedirectUrl)}";

            var OAuth2 = new OAuth2Service(apiUrl, "clientId", "clientSectet");

            var actual = OAuth2.GetAuthorizationUrl(redirect);

            Assert.AreEqual(RelativeAuthUrl, actual.AbsolutePath);
            Assert.AreEqual(expectedUrl, actual.AbsoluteUri);
            Assert.AreEqual(expectedHost, actual.Host);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CannotRefreshEmptyToken()
        {
            var OAuth2 = new OAuth2Service("clientId", "clientSecret");
            var error = OAuth2.RefreshTokenAsync(null).Result;

            Assert.IsNotInstanceOfType(error, typeof(Token));
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void CannotValidateEmptyToken()
        {
            var OAuth2 = new OAuth2Service("clientId", "clientSecret");
            var error = OAuth2.ValidateTokenAsync(null).Result;

            Assert.IsNotInstanceOfType(error, typeof(Token));
        }
    }
}
