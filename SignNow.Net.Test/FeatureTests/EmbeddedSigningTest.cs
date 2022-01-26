using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task CreatesEmbeddedSigningInviteForDocument()
        {
            DisposableDocumentId = UploadTestDocumentWithFieldExtract(
                PdfFilePath, "CreatesEmbeddedSigningInviteForDocument.pdf");
            var document = await SignNowTestContext.Documents.GetDocumentAsync(DisposableDocumentId).ConfigureAwait(false);

            var invite = new EmbeddedSigningInvite(document);
            invite.AddEmbeddedSigningInvite(
                new EmbeddedInvite
                {
                    Email = "test.email+embedded@gmail.com",
                    RoleId = document.Roles[0].Id,
                    SigningOrder = 1
                });

            // Creates an Embedded Signing Invite
            var embeddedSigningInvite = await SignNowTestContext.Invites
                .CreateInviteAsync(document.Id, invite)
                .ConfigureAwait(false);

            Assert.IsNotNull(embeddedSigningInvite);
            Assert.AreEqual(1, embeddedSigningInvite.InviteData.Count);

            // Generate Link for Embedded Signing
            var documentWithEmbed = await SignNowTestContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);
            Assert.IsTrue(documentWithEmbed.FieldInvites.First().IsEmbedded);

            var linkOptions = new CreateEmbedLinkOptions
            {
                FieldInvite = documentWithEmbed.FieldInvites.First(),
                LinkExpiration = 30
            };

            var embeddedLink = await SignNowTestContext.Invites
                .GenerateEmbeddedInviteLinkAsync(document.Id, linkOptions)
                .ConfigureAwait(false);

            Assert.IsNotNull(embeddedLink);
            Assert.IsInstanceOfType(embeddedLink.Link, typeof(Uri));

            // Cancel embedded invite
            await SignNowTestContext.Invites.CancelEmbeddedInviteAsync(documentWithEmbed.Id).ConfigureAwait(false);

            var documentWithoutEmbed = await SignNowTestContext.Documents.GetDocumentAsync(documentWithEmbed.Id).ConfigureAwait(false);

            Assert.AreEqual(0, documentWithoutEmbed.FieldInvites.Count);
        }
    }
}
