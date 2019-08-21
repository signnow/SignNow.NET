using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.Context;
using System;
using System.Threading.Tasks;

namespace SignNow.Net.Test.AcceptanceTests
{
    [TestClass]
    public class GetTokenByPasswordAsyncTest : ApiTestBase
    {
        [TestMethod]
        public void TokenRetrieved()
        {
            var user = new CredentialLoader(ApiBaseUrl).GetCredentials();
            var creds = new CredentialLoader(ApplicationBaseUrl).GetCredentials();

            var authObject = new OAuth2Service(user.Login, user.Password);
            var tokenTask = authObject.GetTokenAsync(creds.Login, creds.Password, Scope.All);

            Task.WaitAll(tokenTask);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");
                        
            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");
        }
    }
}
