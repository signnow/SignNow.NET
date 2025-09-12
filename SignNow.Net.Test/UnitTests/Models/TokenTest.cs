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
        public void ShouldDeserializeFromJson()
        {
            var fakeToken = new TokenFaker().Generate();
            var fakeJson = TestUtils.SerializeToJsonFormatted(fakeToken);

            var actual = TestUtils.DeserializeFromJson<Token>(fakeJson);
            var actualJson = TestUtils.SerializeToJsonFormatted(actual);

            Assert.AreEqual(fakeJson, actualJson);
            Assert.AreEqual(fakeToken.AccessToken, actual.AccessToken);
            Assert.AreEqual(fakeToken.ExpiresIn, actual.ExpiresIn);
            Assert.AreEqual(fakeToken.Scope, actual.Scope);
            Assert.AreEqual(fakeToken.TokenType, actual.TokenType);
            Assert.AreNotEqual(fakeToken.AppToken, actual.AppToken);
        }

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
