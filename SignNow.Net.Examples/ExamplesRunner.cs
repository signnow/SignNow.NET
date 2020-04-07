using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Examples.Authentication;
using SignNow.Net.Test.Context;

namespace SignNow.Net.Examples
{
    [TestClass]
    public class ExamplesRunner
    {
        private static CredentialModel _clientInfo, _userCredentials;

        /// <summary>
        /// SignNow API base Url (sandbox)
        /// </summary>
        private static Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");

        /// <summary>
        /// SignNow Application base Url (sandbox)
        /// </summary>
        private static Uri ApplicationBaseUrl => new Uri("https://app-eval.signnow.com/");

        public ExamplesRunner()
        {
            // Contains application clientId and clientSecret
            _clientInfo = new CredentialLoader(ApiBaseUrl).GetCredentials();
            // Contains user Email and Password
            _userCredentials = new CredentialLoader(ApplicationBaseUrl).GetCredentials();
        }

        [TestMethod]
        public void RequestAccessTokenTest()
        {
            var requestAccessToken = AuthenticationExamples.RequestAccessToken(ApiBaseUrl, _clientInfo, _userCredentials).Result;

            Assert.IsNotNull(requestAccessToken);
            Assert.IsFalse(string.IsNullOrEmpty(requestAccessToken.AccessToken));
            Assert.IsFalse(string.IsNullOrEmpty(requestAccessToken.RefreshToken));
        }
    }
}
