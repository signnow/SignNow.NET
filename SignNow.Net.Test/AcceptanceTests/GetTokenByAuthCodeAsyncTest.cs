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
        private OAuth2Service authObjectParam2, authObjectParam3;
        private string authCode = "fac566fb5d926c7dd590f6eea7f8b6c10cbe469b";

        /// <summary>
        /// TO DO:  use GetAuthorizationUrlTest to retrieve authorization URL and get AUTH. CODE from HTTP-GET Responce
        /// on this URL and use this code in current test
        /// Make Positive Test
        /// </summary>

        [TestInitialize]
        public void TestInitialize()
        {
            clientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
            userCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();
            authObjectParam2 = new OAuth2Service(clientInfo.Login, clientInfo.Password);
            authObjectParam3 = new OAuth2Service(ApiBaseUrl, clientInfo.Login, clientInfo.Password);
        }

        [TestMethod]
        public void Ctor2Params_TokenRetrieving_Fail_WrongAuthCode()
        {
            var tokenTask = authObjectParam2.GetTokenAsync("wrong_auth_code", Scope.All);

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
        public void Ctor3Params_TokenRetrieving_Fail_WrongAuthCode()
        {
            var tokenTask = authObjectParam3.GetTokenAsync("wrong_auth_code", Scope.All);

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