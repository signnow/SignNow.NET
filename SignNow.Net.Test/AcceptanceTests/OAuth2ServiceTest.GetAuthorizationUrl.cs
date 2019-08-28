using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Test;
using SignNow.Net.Test.Context;
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
            var redirectUrl = new Uri("https://localhost:1000/");

            var authObject = new OAuth2Service(clientId, clientSecter);
            var uri = authObject.GetAuthorizationUrl(redirectUrl);

            if (String.IsNullOrEmpty(uri.ToString()))
                Assert.Fail("Authorization Url is empty");

            Assert.AreEqual(uri.Scheme, "https");

            Assert.AreEqual(uri.Query, $"?client_id={clientId}&response_type=code&redirect_uri={redirectUrl}");

            Assert.AreEqual(uri.LocalPath, "/proxy/index.php/authorize");

            Assert.AreEqual(uri.Host, "eval.signnow.com");
        }
    }
}