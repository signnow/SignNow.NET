using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.SignNow;
using System;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public partial class OAuth2ServiceTest : ApiTestBase
    {
#pragma warning disable CA1031 // Do not catch general exception types

        [TestMethod]
        public void GetTokenByPassword_Success_ScopeAll()
        {
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);
            var tokenTaskExt = authObjectParam3.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

            Task.WaitAll(tokenTask, tokenTaskExt);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");
            Assert.IsFalse(tokenTaskExt.IsFaulted, $"Token retreiving error: {tokenTaskExt.Exception}");

            var token = tokenTask.Result;
            var tokenExt = tokenTaskExt.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
            {
                Assert.Fail("Access token is empty");
            }

            Assert.IsNotNull(tokenExt, "Token is null");
            if (String.IsNullOrEmpty(tokenExt.AccessToken))
            {
                Assert.Fail("Access token is empty");
            }
        }

        [TestMethod]
        public void GetTokenByPassword_Success_ScopeUser()
        {
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.User);
            var tokenTaskExt = authObjectParam3.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.User);

            Task.WaitAll(tokenTask, tokenTaskExt);

            Assert.IsFalse(tokenTask.IsFaulted, $"Token retreiving error: {tokenTask.Exception}");
            Assert.IsFalse(tokenTaskExt.IsFaulted, $"Token retreiving error: {tokenTaskExt.Exception}");

            var token = tokenTask.Result;
            var tokenExt = tokenTaskExt.Result;

            Assert.IsNotNull(token, "Token is null");
            if (String.IsNullOrEmpty(token.AccessToken))
                Assert.Fail("Access token is empty");

            Assert.IsNotNull(tokenExt, "Token is null");
            if (String.IsNullOrEmpty(tokenExt.AccessToken))
                Assert.Fail("Access token is empty");
        }

        [TestMethod]
        public void GetTokenByPassword_Fail_WrongPassword()
        {
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, "wrong_password", Scope.All);
            var tokenTaskExt = authObjectParam3.GetTokenAsync(userCredentials.Login, "wrong_password", Scope.All);

            try
            {
                Task.WaitAll(tokenTask, tokenTaskExt);
                Assert.Fail($"Expected error \"Invalid credentials.\" with wrong user password. Recieved Exception: {tokenTask.Exception}");
                Assert.Fail($"Expected error \"Invalid credentials.\" with wrong user password. Recieved Exception: {tokenTaskExt.Exception}");
            }
            catch
            {
                Assert.AreEqual(tokenTask.Exception.InnerException.Message, "Invalid credentials.");
                Assert.AreEqual(tokenTaskExt.Exception.InnerException.Message, "Invalid credentials.");
            }
        }

        [TestMethod]
        public void GetTokenByPassword_Fail_WrongClientSecret()
        {
            authObjectParam2 = new OAuth2Service(clientInfo.Login, "client_secret_wrong");
            authObjectParam3 = new OAuth2Service(ApiBaseUrl, clientInfo.Login, "client_secret_wrong");
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);            
            var tokenTaskExt = authObjectParam3.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

            try
            {
                Task.WaitAll(tokenTask, tokenTaskExt);
                Assert.Fail($"Expected error \"invalid_client\" with wrong user's client secret. Recieved Exception: {tokenTask.Exception}");
                Assert.Fail($"Expected error \"invalid_client\" with wrong user's client secret. Recieved Exception: {tokenTaskExt.Exception}");
            }
            catch
            {
                Assert.AreEqual(tokenTask.Exception.InnerException.Message, "invalid_client");
                Assert.AreEqual(tokenTaskExt.Exception.InnerException.Message, "invalid_client");
            }
        }
#pragma warning restore CA1031 // Do not catch general exception types

    }
}