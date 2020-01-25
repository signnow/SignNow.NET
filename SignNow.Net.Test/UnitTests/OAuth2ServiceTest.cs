using System;
using System.Globalization;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Test.Constants;

namespace UnitTests
{
    [TestClass]
    public class OAuth2ServiceTest
    {
        private readonly string RedirectUrl = "https://github.com/signnow/SignNow.NET";
        private readonly string RelativeAuthUrl = "/proxy/index.php/authorize";

        private OAuth2Service OAuth2 { get; set; }

        [TestInitialize]
        public void SetUp()
        {
            OAuth2 = new OAuth2Service("clientId", "clientSecret");
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

            var actual = OAuth2.GetAuthorizationUrl(redirect);

            Assert.AreEqual("https", actual.Scheme);
            Assert.AreEqual(expectedHost, actual.Host);
            Assert.AreEqual(expectedUrl, actual.AbsoluteUri);
            Assert.AreEqual(RelativeAuthUrl, actual.AbsolutePath);
            Assert.AreEqual("?client_id=clientId&response_type=code&redirect_uri=https%3A%2F%2Fgithub.com%2Fsignnow%2FSignNow.NET", actual.Query);
            Assert.AreEqual(RelativeAuthUrl, actual.LocalPath);
        }

        [TestMethod]
        public void CannotGetAuthUrlWithoutRedirectUrl()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => OAuth2.GetAuthorizationUrl(null));

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, ErrorMessages.ValueCannotBeNull, "redirectUrl"),
                exception.Message);
        }

        [TestMethod]
        public void CannotRefreshEmptyToken()
        {
            var exception = Assert.ThrowsException<AggregateException>(
                () => OAuth2.RefreshTokenAsync(null).Result);

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, ErrorMessages.ValueCannotBeNull, "token"),
                exception.InnerException?.Message);
        }

        [TestMethod]
        public void CannotValidateEmptyToken()
        {
            var exception = Assert.ThrowsException<AggregateException>(
                () => OAuth2.ValidateTokenAsync(null).Result);

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, ErrorMessages.ValueCannotBeNull, "token"),
                exception.InnerException?.Message);
        }
    }
}
