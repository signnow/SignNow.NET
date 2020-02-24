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
            var baseDocument = new SignNowDocumentFaker()
                .RuleFor(d => d.Roles, f => new RoleFaker().Generate(1));

            var document = baseDocument.Generate();
            var inviteFirst = new RoleBasedInvite(document);
            inviteFirst.AddRoleBasedInvite(new SignerOptions(signer1, inviteFirst.DocumentRoles().First()));

            // assertion for invite
            Assert.AreEqual(1, inviteFirst.DocumentRoles().Count);

            // create document with one role and one role invite and check the document status
            var documentWithOneInvite = baseDocument
                .RuleFor(d => d.FieldInvites, f => new FieldInviteFaker().Generate(1))
                .RuleFor(d => d.Fields, f => new FieldFaker().Generate(1))
                .FinishWith((f, obj) => {
                    var role = obj.Roles.GetEnumerator();
                    var invite = obj.FieldInvites.GetEnumerator();

                    foreach(var field in obj.Fields)
                    {
                        role.MoveNext();
                        invite.MoveNext();

                        field.RoleName = role.Current.Name;
                        field.RoleId = role.Current.Id;
                        field.Owner = obj.Owner;
                        field.Signer = signer1;
                        invite.Current.RoleId = role.Current.Id;
                        invite.Current.SignerEmail = signer1;
                        invite.Current.Status = InviteStatus.Pending;
                    }
                })
                .Generate();

            var fieldInvite = documentWithOneInvite.FieldInvites.First();

            Assert.AreEqual(InviteStatus.Pending, fieldInvite.Status);
            Assert.AreEqual(documentWithOneInvite.Roles.First().Name, fieldInvite.RoleName);
            Assert.AreEqual(documentWithOneInvite.Roles.First().Id, fieldInvite.RoleId);
            Assert.AreEqual(signer1, fieldInvite.SignerEmail);
            Assert.AreEqual(InviteStatus.Pending, fieldInvite.Status);
            Assert.AreEqual(DocumentStatus.Pending, documentWithOneInvite.Status);

            // sign the document and check the document status
            var documentSigned = baseDocument
                .RuleFor(d => d.Signatures, new SignatureContentFaker().Generate(1))
                .RuleFor(d => d.FieldInvites, f => new FieldInviteFaker().Generate(1))
                .RuleFor(d => d.Fields, f => new FieldFaker().Generate(1))
                .FinishWith((f, obj) => {
                    var role = obj.Roles.GetEnumerator();
                    var invite = obj.FieldInvites.GetEnumerator();
                    var sign = obj.Signatures.GetEnumerator();

                    foreach (var field in obj.Fields)
                    {
                        role.MoveNext();
                        invite.MoveNext();
                        sign.MoveNext();

                        field.RoleName = role.Current.Name;
                        field.RoleId = role.Current.Id;
                        field.Owner = obj.Owner;
                        field.Signer = signer1;
                        invite.Current.RoleId = role.Current.Id;
                        invite.Current.SignerEmail = signer1;
                        invite.Current.Status = InviteStatus.Fulfilled;
                        sign.Current.Email = signer1;
                        sign.Current.SignatureRequestId = invite.Current.Id;
                    }
                })
                .Generate();

            Assert.AreEqual(InviteStatus.Fulfilled, documentSigned.FieldInvites.First().Status);
            Assert.AreEqual(DocumentStatus.Completed, documentSigned.Status);
        }
    }
}
