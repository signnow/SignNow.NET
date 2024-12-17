using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples
{
    [TestClass]
    public partial class InviteExamples : ExamplesRunner
    {
        [TestMethod]
        public async Task CreateEmbeddedSigningInviteToSignTheDocumentAsync()
        {
            // Upload document with signature field
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateEmbeddedSigningInviteToSignTheDocument.pdf")
                .ConfigureAwait(false);

            // Get document with signature field
            var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);

            // Create Embedded Signing Invite
            var invite = new EmbeddedSigningInvite(signNowDoc);
            invite.AddEmbeddedSigningInvite(
                new EmbeddedInvite
                {
                    Email = "testemail@signnow.com",
                    RoleId = signNowDoc.Roles[0].Id,
                    SigningOrder = 1
                });
            var embeddedInviteResponse = await testContext.Invites
                .CreateInviteAsync(signNowDoc.Id, invite)
                .ConfigureAwait(false);

            // Check if invite was created
            Assert.AreEqual(1, embeddedInviteResponse.InviteData.Count);
            Assert.AreEqual(1, embeddedInviteResponse.InviteData[0].Order);
            Assert.AreEqual("testemail@signnow.com", embeddedInviteResponse.InviteData[0].Email);
            Assert.AreEqual("Pending", embeddedInviteResponse.InviteData[0].Status.ToString());

            // get document with embedded invite
            var documentWithEmbed = await testContext.Documents
                .GetDocumentAsync(document.Id)
                .ConfigureAwait(false);

            // Check if invite is embedded
            Assert.IsTrue(documentWithEmbed.FieldInvites.First().IsEmbedded);

            // Generate link for Embedded Signing Invite
            var options = new CreateEmbedLinkOptions
            {
                FieldInvite = documentWithEmbed.FieldInvites.First(),
                LinkExpiration = 30
            };

            var embeddedLink = await testContext.Invites
                .GenerateEmbeddedInviteLinkAsync(documentWithEmbed.Id, options)
                .ConfigureAwait(false);

            // Check if link is generated
            Assert.IsInstanceOfType(embeddedLink.Link, typeof(Uri));
            Console.WriteLine($@"Embedded link: {embeddedLink.Link.AbsoluteUri}");

            // Cancel embedded invite to delete test document with embedded invite
            await testContext.Invites.CancelEmbeddedInviteAsync(documentWithEmbed.Id).ConfigureAwait(false);

            // clean up
            DeleteTestDocument(document.Id);
            DeleteTestDocument(documentWithEmbed.Id);
        }
    }
}
