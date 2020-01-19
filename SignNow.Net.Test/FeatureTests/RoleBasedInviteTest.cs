using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Test;

namespace FeatureTests
{
    [TestClass]
    public class RoleBasedInviteTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void DocumentOwnerCanSendRoleBasedInvite()
        {
            // Init all required data: User, Document, Invite
            var signNow = new SignNowContext(Token);

            using (var fileStream = File.OpenRead(PdfFilePath))
            {
                DocumentId = signNow.Documents.UploadDocumentWithFieldExtractAsync(fileStream, pdfFileName).Result.Id;
            }

            // Upload document and extract fields
            var document = signNow.Documents.GetDocumentAsync(DocumentId).Result;

            // Create role-based invite
            var invite = new RoleBasedInvite(document);

            Assert.AreEqual(1, invite.DocumentRoles().Count);
            Assert.AreEqual("Signer 1", invite.DocumentRoles().First().Name);

            // Attach signer email to existing role in the document
            invite.AddRoleBasedInvite("signer1@signnow.com", invite.DocumentRoles().First());

            // Send invite request
            var inviteResponse = signNow.Invites.CreateInviteAsync(DocumentId, invite).Result;
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            // Check role-based invite status in the document
            var documentUpdated = signNow.Documents.GetDocumentAsync(DocumentId).Result;
            var fieldInvites = documentUpdated.FieldInvites.First();

            Assert.AreEqual("Pending", fieldInvites.Status.ToString());
            Assert.AreEqual("Signer 1", fieldInvites.RoleName);
            Assert.AreEqual("signer1@signnow.com", fieldInvites.Email);
        }
    }
}
