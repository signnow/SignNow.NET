using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    public partial class OAuth2ServiceTest : ApiTestBase
    {
        [TestMethod]
        public void Should_Refresh_Token()
        {
            var tokenTask = authObjectParam2.GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All);

            Task.WaitAll(tokenTask);

            var token = tokenTask.Result;

            Assert.IsNotNull(token, "Token is null");
            Assert.IsFalse(String.IsNullOrEmpty(token.AccessToken));
            Assert.IsFalse(String.IsNullOrEmpty(token.RefreshToken));

            var refreshTask = authObjectParam2.RefreshTokenAsync(token);

            Task.WaitAll(refreshTask);

            var freshToken = refreshTask.Result;

            Assert.IsNotNull(freshToken, "Token is null");
            Assert.AreNotSame(token.AccessToken, freshToken.AccessToken, "Token is not refreshed");
        }
    }
}
