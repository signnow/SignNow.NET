using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace FeatureTests
{
    [TestClass]
    public class RoleBasedInviteTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task DocumentOwnerCanSendRoleBasedInvite()
        {
            DisposableDocumentId = UploadTestDocumentWithFieldExtract(PdfFilePath, "DocumentOwnerCanSendRoleBasedInvite.pdf");
            var document = await SignNowTestContext.Documents.GetDocumentAsync(DisposableDocumentId).ConfigureAwait(false);

            // Create role-based invite
            var invite = new RoleBasedInvite(document);
            var cc = new UserSignNowFaker().Generate();
            invite.AddCcRecipients(cc.Email);

            Assert.AreEqual(1, invite.DocumentRoles().Count, "Expected one Role in the Document.");
            Assert.AreEqual("Signer 1", invite.DocumentRoles().First().Name, "Signer role name mismatch.");

            // Attach signer email to existing role in the document
            invite.AddRoleBasedInvite(
                new SignerOptions("signer1@signnow.com", invite.DocumentRoles().First()));

            // Send invite request
            var inviteResponse = await SignNowTestContext.Invites.CreateInviteAsync(DisposableDocumentId, invite).ConfigureAwait(false);
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            // Check role-based invite status in the document
            var documentUpdated = await SignNowTestContext.Documents.GetDocumentAsync(DisposableDocumentId).ConfigureAwait(false);
            var fieldInvites = documentUpdated.FieldInvites.First();

            Assert.AreEqual("Pending", fieldInvites.Status.ToString(), "Newly created Invite must have status: pending.");
            Assert.AreEqual("Signer 1", fieldInvites.RoleName, "Signer role mismatch.");
            Assert.AreEqual("signer1@signnow.com", fieldInvites.SignerEmail, "Signer email mismatch.");

            // Could not delete document with pending invites. Invites must be cancelled first
            await SignNowTestContext.Invites.CancelInviteAsync(documentUpdated.Id).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task DocumentOwnerShouldCancelFieldInvite()
        {
            DisposableDocumentId = UploadTestDocumentWithFieldExtract(PdfFilePath, "DocumentOwnerShouldCancelFieldInvite.pdf");
            var document = await SignNowTestContext.Documents.GetDocumentAsync(DisposableDocumentId).ConfigureAwait(false);

            // Create role-based invite
            var invite = new RoleBasedInvite(document);

            // Attach signer email to existing role in the document
            invite.AddRoleBasedInvite(
                new SignerOptions("signer1@signnow.com", invite.DocumentRoles().First())
            );

            // Send invite request
            var inviteResponse = await SignNowTestContext.Invites.CreateInviteAsync(DisposableDocumentId, invite).ConfigureAwait(false);
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");

            // Check role-based invite status in the document
            var documentUpdated = await SignNowTestContext.Documents.GetDocumentAsync(DisposableDocumentId).ConfigureAwait(false);
            var fieldInvites = documentUpdated.FieldInvites.First();

            Assert.AreEqual("Pending", fieldInvites.Status.ToString());

            // Cancel role-based invite for the document
            await SignNowTestContext.Invites.CancelInviteAsync(DisposableDocumentId).ConfigureAwait(false);

            var documentCanceled = await SignNowTestContext.Documents.GetDocumentAsync(DisposableDocumentId).ConfigureAwait(false);

            Assert.AreEqual(0, documentCanceled.FieldInvites.Count, "Field invites are not canceled.");
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
                .RuleFor(d => d.fields, f => new FieldFaker().Generate(1))
                .FinishWith((f, obj) => {
                    using var role = obj.Roles.GetEnumerator();
                    using var invite = obj.FieldInvites.GetEnumerator();

                    foreach(var fieldMeta in obj.Fields)
                    {
                        var field = (Field) fieldMeta;
                        role.MoveNext();
                        invite.MoveNext();

                        field.RoleName = role.Current?.Name;
                        field.RoleId = role.Current?.Id;
                        field.Owner = obj.Owner;
                        field.Signer = signer1;
                        invite.Current.RoleId = role.Current?.Id;
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
                    using var role = obj.Roles.GetEnumerator();
                    using var invite = obj.FieldInvites.GetEnumerator();
                    using var sign = obj.Signatures.GetEnumerator();

                    foreach (var fieldMeta in obj.Fields)
                    {
                        var field = (Field) fieldMeta;
                        role.MoveNext();
                        invite.MoveNext();
                        sign.MoveNext();

                        field.RoleName = role.Current?.Name;
                        field.RoleId = role.Current?.Id;
                        field.Owner = obj.Owner;
                        field.Signer = signer1;
                        invite.Current.RoleId = role.Current?.Id;
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
