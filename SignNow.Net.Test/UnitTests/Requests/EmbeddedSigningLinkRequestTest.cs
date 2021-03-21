using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model.Requests;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class EmbeddedSigningLinkRequestTest
    {
        [TestMethod]
        public void ShouldProperCreateJsonRequest()
        {
            var options = new CreateEmbedLinkOptions
            {
                AuthTypeMethodAsync = EmbeddedAuthType.None,
                FieldInvite = new FieldInviteFaker().Generate(),
                LinkExpiration = null
            };
            var embedLinkRequest = new EmbeddedSigningLinkRequest(options);

            var jsonBody = embedLinkRequest.GetHttpContent().ReadAsStringAsync().Result;
            var actualEmbeddedLink = TestUtils.DeserializeFromJson<CreateEmbedLinkOptions>(jsonBody);

            Assert.AreEqual("application/json", embedLinkRequest.GetHttpContent().Headers.ContentType.MediaType);
            Assert.AreEqual(EmbeddedAuthType.None, actualEmbeddedLink.AuthTypeMethodAsync);
            Assert.AreEqual(null, actualEmbeddedLink.LinkExpiration);
        }
    }
}
