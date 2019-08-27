using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Test;
using SignNow.Net.Test.Context;
using System;

namespace AcceptanceTests
{
    [TestClass]
    public class OAuth2ServiceTest_AuthorizationUrl : ApiTestBase
    {
        [TestMethod]
        public void GetAuthorizationUrlAsync_UrlValidated()
        {
            var user = new CredentialLoader(ApiBaseUrl).GetCredentials();

            var clientId = user.Login;
            var clientSecter = user.Password;
            var redirectUrl = "https://localhost:1000/";

            var authObject = new OAuth2Service(clientId, clientSecter);
            var uri = authObject.GetAuthorizationUrlAsync(redirectUrl).Result;

            if (String.IsNullOrEmpty(uri.ToString()))
                Assert.Fail("Authorization Url is empty");

            Assert.AreEqual(uri.Scheme, "https");

            Assert.AreEqual(uri.Query, $"?client_id={clientId}&response_type=code&redirect_uri={redirectUrl}");

            Assert.AreEqual(uri.LocalPath, "/proxy/index.php/authorize");
        }
    }
}
