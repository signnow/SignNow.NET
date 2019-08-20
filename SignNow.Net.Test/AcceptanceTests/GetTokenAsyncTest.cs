using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.Context;
using System;
using System.Threading.Tasks;

namespace SignNow.Net.Test.AcceptanceTests
{
    [TestClass]
    public class GetTokenAsyncTest : ApiTestBase
    {
        [TestMethod]
        public void TokenRetriever()
        {
            var authCode = "32e49edbe63c3346f556a896d369d51b660b138e";

            var user = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var authObject = new OAuth2Service(user.Login, user.Password);
            var tokenTask = authObject.GetTokenAsync(authCode, Scope.All);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");

            Task.WhenAll(tokenTask);
            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");
        }
    }
}
