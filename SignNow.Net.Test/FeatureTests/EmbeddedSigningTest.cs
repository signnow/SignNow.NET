using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using UnitTests;

namespace FeatureTests
{
    [TestClass]
    public class EmbeddedSigningTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void CreatesEmbeddedSigningInviteForDocument()
        {
            DisposableDocumentId = UploadTestDocumentWithFieldExtract(
                PdfFilePath, "CreatesEmbeddedSigningInviteForDocument.pdf");
            var document = SignNowTestContext.Documents.GetDocumentAsync(DisposableDocumentId).Result;

            var invite = new EmbeddedSigningInvite(document);
            invite.AddEmbeddedSigningInvite(
                new EmbeddedInvite
                {
                    Email = "test.email+embedded@gmail.com",
                    RoleId = document.Roles[0].Id,
                    SigningOrder = 1
                });

            // Creates an Embedded Signing Invite
            var embeddedSigningInvite = SignNowTestContext.Invites
                .CreateInviteAsync(document.Id, invite)
                .Result;

            TestUtils.Dump(embeddedSigningInvite);

            Assert.IsNotNull(embeddedSigningInvite);
            Assert.AreEqual(1, embeddedSigningInvite.InviteData.Count);

            // Generate Link for Embedded Signing
            var documentWithEmbed = SignNowTestContext.Documents.GetDocumentAsync(document.Id).Result;
            var linkOptions = new CreateEmbedLinkOptions
            {
                FieldInvite = documentWithEmbed.FieldInvites.First(),
                LinkExpiration = 30
            };

            var embeddedLink = SignNowTestContext.Invites
                .GenerateEmbeddedInviteLinkAsync(document.Id, linkOptions)
                .Result;

            Assert.IsNotNull(embeddedLink);
        }
    }
}
