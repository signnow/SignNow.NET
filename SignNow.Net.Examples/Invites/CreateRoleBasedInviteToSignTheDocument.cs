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
        public async Task CreateRoleBasedInviteToSignTheDocumentAsync()
        {
            // Upload a document with a signature field
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateRoleBasedInviteToSignTheDocument.pdf")
                .ConfigureAwait(false);

            // Get the document by Id
            var signNowDoc = await testContext.Documents.GetDocumentAsync(document.Id).ConfigureAwait(false);

            // check if the document doesn't have any invites
            Assert.AreEqual(DocumentStatus.NoInvite, signNowDoc.Status);

            // Create a role-based invite to the document for signature
            var email = "noreply@signnow.com";
            var invite = new RoleBasedInvite(signNowDoc)
            {
                Message = $"{email} invited you to sign the document {signNowDoc.Name}",
                Subject = "The subject of the Email"
            };

            // Creates options for signers
            var signer = new SignerOptions(email, invite.DocumentRoles().First())
                {
                    ExpirationDays = 15,
                    RemindAfterDays = 7,
                }
                .SetAuthenticationByPassword("***PASSWORD_TO_OPEN_THE_DOCUMENT***");

            // Attach signer to existing roles in the document
            invite.AddRoleBasedInvite(signer);

            // Creating Invite request
            var inviteResponse = await testContext.Invites
                .CreateInviteAsync(signNowDoc.Id, invite)
                .ConfigureAwait(false);

            // check if the invite has been created successfully
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            // Get the document by Id to check the status of the invite
            var documentWithInvite = await testContext.Documents.GetDocumentAsync(signNowDoc.Id).ConfigureAwait(false);
            var createdInvite = documentWithInvite.FieldInvites.FirstOrDefault();

            // check if the document has an invite
            var fieldInvite = documentWithInvite.Fields.FirstOrDefault();
            Assert.IsNotNull(fieldInvite?.FieldRequestId);

            // Resend the invite - just for the sake of the example
            await testContext.Invites
                .ResendEmailInviteAsync(fieldInvite?.FieldRequestId)
                .ConfigureAwait(false);

            // check if the document has an invite
            Assert.AreEqual("noreply@signnow.com", createdInvite?.SignerEmail);
            Assert.AreEqual("Signer 1", createdInvite?.RoleName, "Signer role mismatch.");
            Assert.AreEqual(InviteStatus.Pending, createdInvite?.Status);
            Assert.AreEqual(DocumentStatus.Pending, documentWithInvite.Status);

            // cancel the invite to delete the document
            await testContext.Invites
                .CancelInviteAsync(documentWithInvite.Id)
                .ConfigureAwait(false);

            // clean up
            DeleteTestDocument(signNowDoc.Id);
        }
    }
}
