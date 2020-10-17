using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.Context;

namespace AcceptanceTests
{
    [TestClass]
    public sealed class OAuth2ServiceTest : SignNowTestBase
    {
        private static CredentialModel _clientInfo, _userCredentials;
        private OAuth2Service oAuthTest;

        public OAuth2ServiceTest()
        {
            _clientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
            _userCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();
        }

        [TestInitialize]
        public void SetUp()
        {
            oAuthTest = new OAuth2Service(_clientInfo.Login, _clientInfo.Password);
        }

        [DataTestMethod]
        [DataRow(Scope.All, DisplayName = "scope All")]
        [DataRow(Scope.User, DisplayName = "scope User")]
        public void ShouldGetTokenByUserCredentials(Scope scope)
        {
            var token = oAuthTest.GetTokenAsync(_userCredentials.Login, _userCredentials.Password, scope).Result;

            Assert.IsNotNull(token);
            Assert.IsFalse(string.IsNullOrEmpty(token.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(token.RefreshToken));
            Assert.AreEqual("Bearer", token.TokenType.ToString());
            Assert.AreEqual(scope.AsString(), token.Scope);
            Assert.IsTrue(token.ExpiresIn > 0);
            Assert.IsTrue(token.LastLogin > 0);
        }

        [TestMethod]
        public void CannotIssueTokenWithWrongAuthCode()
        {
            var exception = Assert.ThrowsException<AggregateException>(
                () => oAuthTest.GetTokenAsync("wrong_auth_code", Scope.All).Result);
#if NETFRAMEWORK
            Assert.AreEqual("One or more errors occurred.", exception.Message);
#else
            Assert.AreEqual("One or more errors occurred. (internal api error)", exception.Message);
#endif
            Assert.AreEqual("internal api error", exception.InnerException?.Message);
        }

        [TestMethod]
        public void CannotIssueTokenWithWrongClientSecret()
        {
            oAuthTest = new OAuth2Service(_clientInfo.Login, "wrong_client_secret");
            var exception = Assert.ThrowsException<AggregateException>(
                () => oAuthTest.GetTokenAsync(_userCredentials.Login, _userCredentials.Password, Scope.All).Result);
#if NETFRAMEWORK
            Assert.AreEqual("One or more errors occurred.", exception.Message);
#else
            Assert.AreEqual("One or more errors occurred. (invalid_client)", exception.Message);
#endif
            Assert.AreEqual("invalid_client", exception.InnerException?.Message);
        }

        [DataTestMethod]
        [DynamicData(nameof(CredentialsProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(WrongCredentialsNames))]
        public void CannotIssueTokenWithWrongCredentials(string login, string pass, Scope scope, string expected, string testName)
        {
            var exceptionLogin = Assert.ThrowsException<AggregateException>(
                () => oAuthTest.GetTokenAsync(login,pass, scope).Result);
#if NETFRAMEWORK
            Assert.AreEqual($"One or more errors occurred.", exceptionLogin.Message);
#else
            Assert.AreEqual($"One or more errors occurred. ({expected})", exceptionLogin.Message);
#endif
            Assert.AreEqual(expected, exceptionLogin.InnerException?.Message);
        }

        /// <summary>
        /// Data provider for <see cref="CannotIssueTokenWithWrongCredentials"/>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> CredentialsProvider()
        {
            // {login, password, scope, expected, test display name }
            yield return new object[] { "wrongLogin", _userCredentials.Password, Scope.All, "invalid_request", "wrong login, scope.All"};
            yield return new object[] { "wrongLogin", _userCredentials.Password, Scope.User, "invalid_request", "wrong login, scope.User"};
            yield return new object[] { _userCredentials.Login, "wrong_password", Scope.All, "Invalid credentials.", "wrong password, scope.All"};
            yield return new object[] { _userCredentials.Login, "wrong_password", Scope.User, "Invalid credentials.", "wrong password, scope.User"};
        }

        public static string WrongCredentialsNames(MethodInfo methodInfo, object[] data)
        {
            return data.Last().ToString();
        }

        [TestMethod]
        public void ShouldRefreshCurrentToken()
        {
            var token = oAuthTest.GetTokenAsync(_userCredentials.Login, _userCredentials.Password, Scope.All).Result;
            var freshToken = oAuthTest.RefreshTokenAsync(token).Result;

            Assert.IsNotNull(freshToken, "Token is null");
            Assert.AreNotSame(token.AccessToken, freshToken.AccessToken, "Token is not refreshed");
        }

        [TestMethod]
        public void ShouldValidateAccessToken()
        {
            var token = oAuthTest.GetTokenAsync(_userCredentials.Login, _userCredentials.Password, Scope.All).Result;

            Assert.IsTrue(oAuthTest.ValidateTokenAsync(token).Result);

            var wrongToken = new Token
            {
                AccessToken = "wrongToken",
                RefreshToken = "wrongRefreshToken",
                Scope = Scope.All.ToString()
            };
            Assert.IsFalse(oAuthTest.ValidateTokenAsync(wrongToken).Result);
        }
    }
}
