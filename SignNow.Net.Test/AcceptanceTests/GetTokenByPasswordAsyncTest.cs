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
    public class GetTokenByPasswordAsyncTest : ApiTestBase
    {
        private const string BAD_REQUEST = "Bad Request";

        private CredentialModel clientInfo, userCredentials;
        private OAuth2Service authObjectParam2, authObjectParam3;

        [TestInitialize]
        public void TestInitialize()
        {
            clientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
            userCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();
            authObjectParam2 = new OAuth2Service(clientInfo.Login, clientInfo.Password);
            authObjectParam3 = new OAuth2Service(ApiBaseUrl, clientInfo.Login, clientInfo.Password);
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Success_ScopeAll()
        {
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

            Task.WaitAll(tokenTask);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");

            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Success_ScopeUser()
        {
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.User);

            Task.WaitAll(tokenTask);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");

            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Fail_WrongPassword()
        {
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, "wrong_password", Scope.All);

            try
            {
                Task.WaitAll(tokenTask);
                Assert.Fail($"Expected error \"{BAD_REQUEST}\" with wrong user paassword. Recieved Exception: {tokenTask.Exception}");
            }
            catch
            {
                Assert.AreEqual(tokenTask.Exception.InnerException.Message, BAD_REQUEST);
            }
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Fail_WrongClientSecret()
        {
            authObjectParam2 = new OAuth2Service(clientInfo.Login, "client_secret_wrong");
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

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

        [TestMethod]
        public void Ctor3Params_TokenRetrieving_Success_ScopeAll()
        {
            var tokenTask = authObjectParam3.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

            Task.WaitAll(tokenTask);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");

            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");
        }

        [TestMethod]
        public void Ctor3Params_TokenRetrieving_Success_ScopeUser()
        {
            var tokenTask = authObjectParam3.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.User);

            Task.WaitAll(tokenTask);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");

            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");
        }

        [TestMethod]
        public void Ctor3Params_TokenRetrieving_Fail_WrongPassword()
        {
            var tokenTask = authObjectParam3.GetTokenAsync(userCredentials.Login, "wrong_password", Scope.All);

            try
            {
                Task.WaitAll(tokenTask);
                Assert.Fail($"Expected error \"{BAD_REQUEST}\" with wrong user paassword. Recieved Exception: {tokenTask.Exception}");
            }
            catch
            {
                Assert.AreEqual(tokenTask.Exception.InnerException.Message, BAD_REQUEST);
            }
        }

        [TestMethod]
        public void Ctor3Params_TokenRetrieving_Fail_WrongClientSecret()
        {
            authObjectParam3 = new OAuth2Service(ApiBaseUrl, clientInfo.Login, "client_secret_wrong");
            var tokenTask = authObjectParam3.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

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