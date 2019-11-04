using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    public partial class OAuth2ServiceTest : ApiTestBase
    {
        private Token TestToken { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            TestToken = authObjectParam2.
                GetTokenAsync(userCredentials.Login, userCredentials.Password, Scope.All).
                Result;
        }

        [TestMethod]
        public void Should_Refresh_Token()
        {
            Assert.IsNotNull(TestToken, "Token is null");
            Assert.IsFalse(String.IsNullOrEmpty(TestToken.AccessToken));
            Assert.IsFalse(String.IsNullOrEmpty(TestToken.RefreshToken));

            var freshToken = authObjectParam2.
                RefreshTokenAsync(TestToken).
                Result;

            Assert.IsNotNull(freshToken, "Token is null");
            Assert.AreNotSame(TestToken.AccessToken, freshToken.AccessToken, "Token is not refreshed");
        }

        [TestMethod]
        public void Should_Validate_Access_Token()
        {
            Assert.IsNotNull(TestToken, "Token is null");
            Assert.IsFalse(String.IsNullOrEmpty(TestToken.AccessToken));
            Assert.IsFalse(String.IsNullOrEmpty(TestToken.RefreshToken));

            var validToken = authObjectParam3.
                ValidateTokenAsync(TestToken).
                Result;

            Assert.IsTrue(validToken);
        }
    }
}
