using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Test;
using System;

namespace AcceptanceTests
{
    public partial class OAuth2ServiceTest : ApiTestBase
    {
        [TestMethod]
        public void GetAuthorizationUrl_UrlValidated()
        {
            var clientId = clientInfo.Login;
            var clientSecter = clientInfo.Password;

            var redirectUrl = new Uri("https://localhost?param=5");
          
            var authObject = new OAuth2Service(ApiBaseUrl, clientId, clientSecter);
            var uri = authObject.GetAuthorizationUrl(redirectUrl);

            if (String.IsNullOrEmpty(uri.ToString()))
                Assert.Fail("Authorization Url is empty");

            Assert.AreEqual(uri.Scheme, "https");

            Assert.AreEqual(uri.Query, "?client_id=0fccdbc73581ca0f9bf8c379e6a96813&response_type=code&redirect_uri=https%3A%2F%2Flocalhost%2F%3Fparam%3D5");

            Assert.AreEqual(uri.LocalPath, "/proxy/index.php/authorize");

            Assert.AreEqual(uri.Host, "eval.signnow.com");
        }
    }
}