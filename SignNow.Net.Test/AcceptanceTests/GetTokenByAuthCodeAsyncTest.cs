using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.Context;
using System;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    [TestClass]
    public class GetTokenByAuthCodeAsyncTest : ApiTestBase
    {
        private const string BAD_REQUEST = "Bad Request";

        private CredentialModel clientInfo, userCredentials;
        private OAuth2Service authObject;
        private string authCode = "fac566fb5d926c7dd590f6eea7f8b6c10cbe469b";

        [TestInitialize]
        public void TestInitialize()
        {
            clientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
            userCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();
            authObject = new OAuth2Service(ApiBaseUrl, clientInfo.Login, clientInfo.Password);
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Success_ScopeAll()
        {
            var tokenTask = authObject.GetTokenAsync(authCode, Scope.All);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");

            Task.WhenAll(tokenTask);
            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Success_ScopeUser()
        {
            var tokenTask = authObject.GetTokenAsync(authCode, Scope.User);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");

            Task.WhenAll(tokenTask);
            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Fail_WrongAuthCode()
        {
            var tokenTask = authObject.GetTokenAsync("wrong_auth_code", Scope.All);

            try
            {
                Task.WaitAll(tokenTask);
                Assert.Fail($"Expected error \"{BAD_REQUEST}\" with wrong authorization code. Recieved Exception: {tokenTask.Exception}");
            }
            catch
            {
                Assert.AreEqual(tokenTask.Exception.InnerException.Message, BAD_REQUEST);
            }
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Fail_WrongClientSecret()
        {
            authObject = new OAuth2Service(clientInfo.Login, "client_secret_wrong");
            var tokenTask = authObject.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

            try
            {
                Task.WaitAll(tokenTask);
                Assert.Fail($"Expected error \"{BAD_REQUEST}\" with wrong user's client secret. Recieved Exception: {tokenTask.Exception}");
            }
            catch
            {
                Assert.AreEqual(tokenTask.Exception.InnerException.Message, BAD_REQUEST);
            }
        }
    }
}