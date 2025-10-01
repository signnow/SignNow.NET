using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;

namespace SignNow.Net.Examples
{
    public partial class InviteExamples
    {
        [TestMethod]
        public async Task CreateFreeformInviteToSignTheDocumentAsync()
        {
            // Upload the document
            await using var fileStream = File.OpenRead(PdfWithoutFields);
            var document = await testContext.Documents
                .UploadDocumentAsync(fileStream, "CreateFreeformInviteToSignTheDocument.pdf")
                .ConfigureAwait(false);

            // get the document
            var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);

            // Check the document doesn't have any invites
            Assert.AreEqual(DocumentStatus.NoInvite, signNowDoc.Status);

            // Create an free form invite
            var emailTo = "noreply@signnow.com";
            var invite = new FreeFormSignInvite(emailTo)
            {
                Message = $"{emailTo} invited you to sign the document {signNowDoc.Name}",
                Subject = "The subject of the Email"
            };
            var inviteResponse = await testContext.Invites
                .CreateInviteAsync(signNowDoc.Id, invite)
                .ConfigureAwait(false);

            // Check the invite was created
            Assert.IsFalse(string.IsNullOrEmpty(inviteResponse.Id));

            // get document with invite
            var documentWithInvite = await testContext.Documents.GetDocumentAsync(signNowDoc.Id).ConfigureAwait(false);
            var createdInvite = documentWithInvite.InvitesStatus.FirstOrDefault();

            // Check the invite was added to the document
            Assert.AreEqual("noreply@signnow.com", createdInvite?.SignerEmail);
            Assert.AreEqual(inviteResponse.Id, createdInvite?.Id);
            Assert.AreEqual(InviteStatus.Pending, createdInvite?.Status);
            Assert.AreEqual(DocumentStatus.Pending, documentWithInvite.Status);

            // cancel free form invite
            await testContext.Invites.CancelInviteAsync((FreeformInvite)createdInvite).ConfigureAwait(false);

            // clean up
            DeleteTestDocument(document.Id);
        }
    }
}
