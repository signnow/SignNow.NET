using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Service;
using SignNow.Net.Test.Context;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public sealed class OAuth2ServiceTest : SignNowTestBase
    {
        private static CredentialModel _apiCredentials;
        private OAuth2Service oAuthTest;

        public OAuth2ServiceTest()
        {
            _apiCredentials = new CredentialLoader(ApiBaseUrl).GetCredentials();
        }

        [TestInitialize]
        public void SetUp()
        {
            oAuthTest = new OAuth2Service(_apiCredentials.ClientId, _apiCredentials.ClientSecret);
        }

        [DataTestMethod]
        [DataRow(Scope.All, DisplayName = "scope All")]
        [DataRow(Scope.User, DisplayName = "scope User")]
        public async Task ShouldGetTokenByUserCredentials(Scope scope)
        {
            var token = await oAuthTest
                .GetTokenAsync(_apiCredentials.Login, _apiCredentials.Password, scope)
                .ConfigureAwait(false);

            Assert.IsNotNull(token);
            Assert.IsFalse(string.IsNullOrEmpty(token.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(token.RefreshToken));
            Assert.AreEqual("Bearer", token.TokenType.ToString());
            Assert.AreEqual(scope.AsString(), token.Scope);
            Assert.IsTrue(token.ExpiresIn > 0);
            Assert.IsTrue(token.LastLogin > 0);
        }

        [TestMethod]
        public async Task CannotIssueTokenWithWrongAuthCode()
        {
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await oAuthTest
                    .GetTokenAsync("wrong_auth_code", Scope.All)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            Assert.AreEqual("internal api error", exception.Message);
        }

        [TestMethod]
        public async Task CannotIssueTokenWithWrongClientSecret()
        {
            oAuthTest = new OAuth2Service(_apiCredentials.ClientId, "wrong_client_secret");
            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await oAuthTest
                    .GetTokenAsync(_apiCredentials.Login, _apiCredentials.Password, Scope.All)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            Assert.AreEqual("invalid_client", exception.Message);
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
            yield return new object[] { "wrongLogin", _apiCredentials.Password, Scope.All, "invalid_request", "wrong login, scope.All"};
            yield return new object[] { "wrongLogin", _apiCredentials.Password, Scope.User, "invalid_request", "wrong login, scope.User"};
            yield return new object[] { _apiCredentials.Login, "wrong_password", Scope.All, "Invalid credentials.", "wrong password, scope.All"};
            yield return new object[] { _apiCredentials.Login, "wrong_password", Scope.User, "Invalid credentials.", "wrong password, scope.User"};
        }

        public static string WrongCredentialsNames(MethodInfo methodInfo, object[] data)
        {
            return data.Last().ToString();
        }

        [TestMethod]
        public async Task ShouldRefreshCurrentToken()
        {
            var token = await oAuthTest
                .GetTokenAsync(_apiCredentials.Login, _apiCredentials.Password, Scope.All)
                .ConfigureAwait(false);
            var freshToken = await oAuthTest.RefreshTokenAsync(token).ConfigureAwait(false);

            Assert.IsNotNull(freshToken, "Token is null");
            Assert.AreNotSame(token.AccessToken, freshToken.AccessToken, "Token is not refreshed");
        }

        [TestMethod]
        public async Task ShouldValidateAccessToken()
        {
            var token = await oAuthTest
                .GetTokenAsync(_apiCredentials.Login, _apiCredentials.Password, Scope.All)
                .ConfigureAwait(false);

            Assert.IsTrue(oAuthTest.ValidateTokenAsync(token).Result);

            var wrongToken = new Token
            {
                AccessToken = "wrongToken",
                RefreshToken = "wrongRefreshToken",
                Scope = Scope.All.ToString()
            };
            Assert.IsFalse(oAuthTest.ValidateTokenAsync(wrongToken).Result);
        }

        [TestMethod]
        public async Task GetAccessTokenUsingAuthorizationCode()
        {
            if (String.IsNullOrEmpty(_apiCredentials.AuthorizationCode))
            {
                Assert.Inconclusive("There is no authorization_code to run this test");
            }

            var token = await oAuthTest
                .GetTokenAsync(_apiCredentials.AuthorizationCode, Scope.All)
                .ConfigureAwait(false);

            Assert.IsNotNull(token);
            Assert.IsFalse(string.IsNullOrEmpty(token.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(token.RefreshToken));
            Assert.AreEqual("Bearer", token.TokenType.ToString());
            Assert.AreEqual(Scope.All.AsString(), token.Scope);
            Assert.IsTrue(token.ExpiresIn > 0);
            Assert.IsTrue(token.LastLogin > 0);
        }
    }
}
