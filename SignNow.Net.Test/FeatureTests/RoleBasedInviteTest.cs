using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.FakeModels;

namespace FeatureTests
{
    [TestClass]
    public class RoleBasedInviteTest : AuthorizedApiTestBase
    {
        /// <summary>
        /// SignNow context.
        /// </summary>
        private SignNowContext SignNow { get; set; }

        public void UploadTestDocument()
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
            UploadTestDocument();
            var document = SignNow.Documents.GetDocumentAsync(DocumentId).Result;

            // Create role-based invite
            var invite = new RoleBasedInvite(document);

            Assert.AreEqual(1, invite.DocumentRoles().Count, "Expected one Role in the Document.");
            Assert.AreEqual("Signer 1", invite.DocumentRoles().First().Name, "Signer role name mismatch.");

            // Attach signer email to existing role in the document
            invite.AddRoleBasedInvite(
                new SignerOptions("signer1@signnow.com", invite.DocumentRoles().First())
                );

            // Send invite request
            var inviteResponse = SignNow.Invites.CreateInviteAsync(DocumentId, invite).Result;
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            // Check role-based invite status in the document
            var documentUpdated = SignNow.Documents.GetDocumentAsync(DocumentId).Result;
            var fieldInvites = documentUpdated.FieldInvites.First();

            Assert.AreEqual("Pending", fieldInvites.Status.ToString(), "Newly created Invite must have status: pending.");
            Assert.AreEqual("Signer 1", fieldInvites.RoleName, "Signer role mismatch.");
            Assert.AreEqual("signer1@signnow.com", fieldInvites.SignerEmail, "Signer email mismatch.");
        }

        [TestMethod]
        public void DocumentOwnerShouldCancelFieldInvite()
        {
            UploadTestDocument();
            var document = SignNow.Documents.GetDocumentAsync(DocumentId).Result;

            // Create role-based invite
            var invite = new RoleBasedInvite(document);

            // Attach signer email to existing role in the document
            invite.AddRoleBasedInvite(
                new SignerOptions("signer1@signnow.com", invite.DocumentRoles().First())
            );

            // Send invite request
            var inviteResponse = SignNow.Invites.CreateInviteAsync(DocumentId, invite).Result;
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            // Check role-based invite status in the document
            var documentUpdated = SignNow.Documents.GetDocumentAsync(DocumentId).Result;
            var fieldInvites = documentUpdated.FieldInvites.First();

            Assert.AreEqual("Pending", fieldInvites.Status.ToString());

            // Cancel role-based invite for the document
            var cancelResponse = SignNow.Invites.CancelInviteAsync(DocumentId);
            Task.WaitAll(cancelResponse);

            var documentCanceled = SignNow.Documents.GetDocumentAsync(DocumentId).Result;

            Assert.AreEqual(0, documentCanceled.FieldInvites.Count, "Field invites are not canceled.");
            Assert.IsFalse(cancelResponse.IsFaulted, "Cancel response is not successful.");
        }

        [TestMethod]
        public void ShouldGetDocumentSignedStatusForRoleBasedInvite()
        {
            var signer1 = "signer1@email.com";
            var baseDocument = new SignNowDocumentWithFieldsFaker();
            var document = baseDocument.Generate();

            var inviteFirst = new RoleBasedInvite(document);
            inviteFirst.AddRoleBasedInvite(new SignerOptions(signer1, inviteFirst.DocumentRoles().First()));

            // assertion for invite
            Assert.AreEqual(1, inviteFirst.DocumentRoles().Count);

            // create document with one role and one role invite and check the document status
            var documentWithOneInvite = baseDocument
                .RuleFor(
                    d => d.FieldInvites,
                    f => new FieldInviteFaker()
                        .RuleFor(inv => inv.Status, SignStatus.Pending)
                        .RuleFor(inv => inv.SignerEmail, signer1)
                        .RuleFor(inv => inv.RoleId, document.Roles.First().Id)
                        .Generate(1))
                .RuleFor(
                    d => d.Fields,
                    f => new FieldFaker()
                        .RuleFor(fld => fld.Signer, signer1)
                        .RuleFor(fld => fld.Owner, document.Owner)
                        .RuleFor(fld => fld.RoleId, document.Roles.First().Id)
                        .Generate(1))
                .Generate();

            var fieldInvite = documentWithOneInvite.FieldInvites.First();

            Assert.AreEqual(SignStatus.Pending, fieldInvite.Status);
            Assert.AreEqual(documentWithOneInvite.Roles.First().Name, fieldInvite.RoleName);
            Assert.AreEqual(documentWithOneInvite.Roles.First().Id, fieldInvite.RoleId);
            Assert.AreEqual(signer1, fieldInvite.SignerEmail);
            Assert.AreEqual(SignStatus.Pending, fieldInvite.Status);
            Assert.AreEqual(SignStatus.Pending, documentWithOneInvite.Status);

            // sign the document and check the document status
            var documentSigned = baseDocument
                .RuleFor(
                    d => d.Signatures,
                    new SignatureFaker()
                        .RuleFor(s => s.Email, signer1)
                        .RuleFor(s => s.SignatureRequestId, documentWithOneInvite.FieldInvites.First().Id)
                        .Generate(1))
                .RuleFor(
                    d => d.FieldInvites,
                    f => new FieldInviteFaker()
                        .RuleFor(inv => inv.Id, documentWithOneInvite.FieldInvites.First().Id)
                        .RuleFor(inv => inv.Status, SignStatus.Fulfilled)
                        .RuleFor(inv => inv.SignerEmail, signer1)
                        .RuleFor(inv => inv.RoleId, documentWithOneInvite.Roles.First().Id)
                        .Generate(1))
                .RuleFor(
                    d => d.Fields,
                    f => new FieldFaker()
                        .RuleFor(fld => fld.Signer, signer1)
                        .RuleFor(fld => fld.Owner, documentWithOneInvite.Owner)
                        .RuleFor(fld => fld.RoleId, documentWithOneInvite.Roles.First().Id)
                        .Generate(1))
                .Generate();

            Assert.AreEqual(SignStatus.Fulfilled, documentSigned.FieldInvites.First().Status);
            Assert.AreEqual(SignStatus.Completed, documentSigned.Status);
        }
    }
}
