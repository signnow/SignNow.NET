using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class TokenTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var fakeToken = new TokenFaker().Generate();
            var fakeJson = JsonConvert.SerializeObject(fakeToken, Formatting.Indented);

            var actual = JsonConvert.DeserializeObject<Token>(fakeJson);
            var actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);

            Assert.AreEqual(fakeJson, actualJson);
            Assert.AreEqual(fakeToken.AccessToken, actual.AccessToken);
            Assert.AreEqual(fakeToken.ExpiresIn, actual.ExpiresIn);
            Assert.AreEqual(fakeToken.Scope, actual.Scope);
            Assert.AreEqual(fakeToken.TokenType, actual.TokenType);
        }

        [TestMethod]
        public void ShouldGetAuthorizationHeaderValue()
        {
            var token = new TokenFaker().Generate();
            var expected = $"Bearer {token.AccessToken}";

            Assert.AreEqual(expected, token.GetAuthorizationHeaderValue());
        }
    }
}
