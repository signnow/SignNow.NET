using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Test;

namespace FeatureTests
{
    [TestClass]
    public class RoleBasedInviteTest : AuthorizedApiTestBase
    {
        /// <summary>
        /// SignNow context.
        /// </summary>
        private SignNowContext SignNow { get; set; }

        [TestInitialize]
        public void Setup()
        {
            SignNow = new SignNowContext(Token);

            using (var fileStream = File.OpenRead(PdfFilePath))
            {
                // Upload document and extract fields
                DocumentId = SignNow.Documents.UploadDocumentWithFieldExtractAsync(fileStream, "RoleBasedInviteTest" + pdfFileName).Result.Id;
            }
        }

        [TestMethod]
        public void DocumentOwnerCanSendRoleBasedInvite()
        {
            var document = SignNow.Documents.GetDocumentAsync(DocumentId).Result;

            // Create role-based invite
            var invite = new RoleBasedInvite(document);

            Assert.AreEqual(1, invite.DocumentRoles().Count, "Expected one Role in the Document.");
            Assert.AreEqual("Signer 1", invite.DocumentRoles().First().Name, "Signer role name mismatch.");

            // Attach signer email to existing role in the document
            invite.AddRoleBasedInvite("signer1@signnow.com", invite.DocumentRoles().First());

            // Send invite request
            var inviteResponse = SignNow.Invites.CreateInviteAsync(DocumentId, invite).Result;
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            // Check role-based invite status in the document
            var documentUpdated = SignNow.Documents.GetDocumentAsync(DocumentId).Result;
            var fieldInvites = documentUpdated.FieldInvites.First();

            Assert.AreEqual("Pending", fieldInvites.Status.ToString(), "Newly created Invite must have status: pending.");
            Assert.AreEqual("Signer 1", fieldInvites.RoleName, "Signer role mismatch.");
            Assert.AreEqual("signer1@signnow.com", fieldInvites.Email, "Signer email mismatch.");
        }

        [TestMethod]
        public void DocumentOwnerShouldCancelFieldInvite()
        {
            // Upload document and extract fields
            var document = SignNow.Documents.GetDocumentAsync(DocumentId).Result;
            // Create role-based invite
            var invite = new RoleBasedInvite(document);
            // Attach signer email to existing role in the document
            invite.AddRoleBasedInvite("signer1@signnow.com", invite.DocumentRoles().First());
            // Send invite request
            var inviteResponse = SignNow.Invites.CreateInviteAsync(DocumentId, invite).Result;
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            // Check role-based invite status in the document
            var documentUpdated = SignNow.Documents.GetDocumentAsync(DocumentId).Result;
            var fieldInvites = documentUpdated.FieldInvites.First();

            Assert.AreEqual("Pending", fieldInvites.Status.ToString());

            // Cancel role-based invite for the document
            var cancelResponse = SignNow.Invites.CancelInviteAsync(document);
            Task.WaitAll(cancelResponse);

            var documentCanceled = SignNow.Documents.GetDocumentAsync(DocumentId).Result;
            Assert.AreEqual(0, documentCanceled.FieldInvites.Count, "Field invites are not canceled.");
            Assert.IsFalse(cancelResponse.IsFaulted, "Cancel response is not successful.");
        }
    }
}
