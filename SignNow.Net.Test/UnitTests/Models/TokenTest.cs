using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace UnitTests.Models
{
    [TestClass]
    public class TokenTest
    {

        [TestMethod]
        public void ShouldGetAuthorizationHeaderValue()
        {
            var token = new TokenFaker().Generate();
            var expected = $"Bearer {token.AccessToken}";

            Assert.AreEqual(expected, token.GetAuthorizationHeaderValue());

            token.TokenType = TokenType.Basic;
            token.AppToken = "basic_token";
            Assert.AreEqual("Basic basic_token", token.GetAuthorizationHeaderValue());
        }
    }
}
