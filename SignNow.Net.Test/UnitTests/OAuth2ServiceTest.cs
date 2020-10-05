using System;
using System.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SignNow.Net;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Constants;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Internal.Service;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.Constants;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    /// <summary>
    /// Test <see cref="OAuth2Service"/> object with
    /// </summary>
    public class OAuth2ServiceMock : OAuth2Service
    {
        /// <inheritdoc cref="OAuth2Service"/>
        public OAuth2ServiceMock(Uri apiBaseUrl, string clientId, string clientSecret, ISignNowClient mockClient)
            : base(apiBaseUrl, clientId, clientSecret, mockClient) {}
    }

    [TestClass]
    public class OAuth2ServiceTest : SignNowTestBase
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

            StringAssert.Contains(exception.Message, ErrorMessages.ValueCannotBeNull);
            StringAssert.Contains(exception.ParamName, "redirectUrl");
        }

        [TestMethod]
        public void CannotRefreshEmptyToken()
        {
            var exception = Assert.ThrowsException<AggregateException>(
                () => OAuth2.RefreshTokenAsync(null).Result);
#if NET45
            StringAssert.Contains(exception.Message, "One or more errors occurred.");
#else
            StringAssert.Contains(exception.Message, ErrorMessages.ValueCannotBeNull);
#endif
            StringAssert.Contains(exception.InnerException?.Message, "token");
        }

        [TestMethod]
        public void CannotValidateEmptyToken()
        {
            var exception = Assert.ThrowsException<AggregateException>(
                () => OAuth2.ValidateTokenAsync(null).Result);

#if NET45
            StringAssert.Contains(exception.Message, "One or more errors occurred.");
#else
            StringAssert.Contains(exception.Message, ErrorMessages.ValueCannotBeNull);
#endif
            StringAssert.Contains(exception.InnerException?.Message, "token");
        }

        [TestMethod]
        public void EnsuresTokeLifetimeForCodeGrant()
        {
            // Add 3 days for token lifetime test
            var futureTimestamp = UnixTimeStampConverter.ToUnixTimestamp(DateTime.Now.AddDays(3));
            var fakeToken = new TokenFaker()
                .RuleFor(t => t.ExpiresIn, (int)futureTimestamp)
                .Generate();

            var tokenJsonResponse = TestUtils.SerializeToJsonFormatted(fakeToken);

            // Set up mock
            var mock = new Mock<SignNowClient>(null);
            mock.As<ISignNowClient>()
                .Setup(x => x.RequestAsync<Token>(It.IsAny<RequestOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TestUtils.DeserializeFromJson<Token>(tokenJsonResponse));

            var oauth2 = new OAuth2ServiceMock(ApiBaseUrl, "id", "secret", mock.Object);
            var token = oauth2.GetTokenAsync("some_authorization_code", Scope.All).Result;

            Assert.AreNotEqual(tokenJsonResponse, TestUtils.SerializeToJsonFormatted(token));
            Assert.AreNotEqual(fakeToken.ExpiresIn, token.ExpiresIn);
            Assert.IsTrue(2592000 - token.ExpiresIn >= 0, $"token lifetime adjustment error: {token.ExpiresIn}");
            Assert.IsTrue(fakeToken.ExpiresIn > token.ExpiresIn, "ExpiresIn is Timestamp, expected lifetime");
        }
    }
}
