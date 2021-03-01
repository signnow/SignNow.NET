using System.Collections.Generic;
using System.Linq;
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
                .RuleFor(o => o.Roles, new RoleFaker().Generate(2))
                .RuleFor(o => o.FieldInvites, new FieldInviteFaker().Generate(2))
                .FinishWith((f, o) =>
                    {
                        o.Roles.ForEach(r =>
                            r.Id = o.FieldInvites.First(
                                fi => fi.RoleName == r.Name).RoleId);
                    }
                )
                .Generate();

            var embeddedSigningRequest = new EmbeddedSigningRequest(document);
            var jsonBody = embeddedSigningRequest.GetHttpContent().ReadAsStringAsync().Result;
            var actualEmbeddedSigning = TestUtils.DeserializeFromJson<EmbeddedSigningRequest>(jsonBody);

            TestUtils.Dump(embeddedSigningRequest);
            TestUtils.Dump(document);

            List<FieldInvite> fieldInvites = (List<FieldInvite>) document.FieldInvites;

            Assert.AreEqual("application/json", embeddedSigningRequest.GetHttpContent().Headers.ContentType.MediaType);
            Assert.IsTrue(jsonBody.Contains("invites"));

            Assert.AreEqual(fieldInvites[0].SignerEmail, actualEmbeddedSigning.Invites[0].Email);
            Assert.AreEqual(fieldInvites[1].SignerEmail, actualEmbeddedSigning.Invites[1].Email);
            Assert.AreEqual(fieldInvites[0].RoleId, actualEmbeddedSigning.Invites[0].RoleId);
            Assert.AreEqual(fieldInvites[1].RoleId, actualEmbeddedSigning.Invites[1].RoleId);
            Assert.AreEqual(1, actualEmbeddedSigning.Invites[0].SigningOrder);
            Assert.AreEqual(2, actualEmbeddedSigning.Invites[1].SigningOrder);
        }
    }
}
