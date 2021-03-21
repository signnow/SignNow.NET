using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Requests;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class CreateEmbeddedSigningInviteRequest
    {
        [TestMethod]
        public void ShouldProperCreateJsonRequest()
        {
            // Creates document with two signers and two filed invites
            var document = new SignNowDocumentFaker()
                .RuleFor(o => o.Roles, new RoleFaker().Generate(1))
                .Generate();

            var invite = new EmbeddedSigningInvite(document);
            invite.AddEmbeddedSigningInvite(
                new EmbeddedInvite
                {
                    Email = "email@gmail.com",
                    RoleId = document.Roles[0].Id,
                    SigningOrder = 1
                });

            var embeddedSigningRequest = new EmbeddedSigningRequest(invite);
            var jsonBody = embeddedSigningRequest.GetHttpContent().ReadAsStringAsync().Result;
            var actualEmbeddedSigning = TestUtils.DeserializeFromJson<EmbeddedSigningRequest>(jsonBody);

            TestUtils.Dump(embeddedSigningRequest);

            Assert.AreEqual("application/json", embeddedSigningRequest.GetHttpContent().Headers.ContentType.MediaType);
            Assert.IsTrue(jsonBody.Contains("invites"));
            Assert.AreEqual((uint)1, actualEmbeddedSigning.Invites[0].SigningOrder);
            Assert.AreEqual(document.Roles[0].Id, actualEmbeddedSigning.Invites[0].RoleId);
            Assert.AreEqual("none", actualEmbeddedSigning.Invites[0].AuthMethod);
        }
    }
}
