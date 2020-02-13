using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.FakeModels;

namespace FeatureTests
{
    [TestClass]
    public class FreeFormInviteTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void DocumentOwnerCanSendFreeFormInviteToUser()
        {
            // Init all required data: User, Document, Invite
            var signNow = new SignNowContext(Token);
            var invitee = new UserSignNowFaker().Generate();
            var invite = new FreeFormSignInvite(invitee.Email);

            DocumentId = UploadTestDocument(PdfFilePath, signNow.Documents);

            // Creating Invite request
            var inviteResponseTask = signNow.Invites.CreateInviteAsync(DocumentId, invite);
            Task.WaitAll(inviteResponseTask);

            var inviteResponse = inviteResponseTask.Result;

            Assert.IsFalse(inviteResponseTask.IsFaulted, "Invite request should be created.");
            Assert.AreEqual(invite.Recipient, invitee.Email, "Freeform invite request should contains proper user email.");
            Assert.AreEqual(inviteResponse.Id, inviteResponse.Id.ValidateId(), "Successful invite response should contains valid Invite ID.");

            // Check if invite was successful and the document contains invite request data
            var documentInfo = signNow.Documents.GetDocumentAsync(DocumentId).Result;
            var inviteIdx = documentInfo.InviteRequests.FindIndex(request => request.Id == inviteResponse.Id);
            var documentInviteRequest = documentInfo.InviteRequests[inviteIdx];

            Assert.AreEqual(DocumentId, documentInfo.Id, "You should get proper document details.");
            Assert.AreEqual(inviteResponse.Id, documentInviteRequest.Id, "Document should contains freeform invite ID after invite has been sent.");
            Assert.AreEqual(invitee.Email.ToLowerInvariant(), documentInviteRequest.SignerEmail, "Invite should contains user email whom was sent invite request.");
            Assert.IsNull(documentInviteRequest.IsCanceled, "Invite status should not be canceled by default.");
            Assert.AreEqual(InviteStatus.Pending, documentInviteRequest.Status);
            Assert.AreEqual(DocumentStatus.Pending, documentInfo.Status);
        }

        [TestMethod]
        public void ShouldGetDocumentSignedStatusForFreeFormInvite()
        {
            var baseDocument = new SignNowDocumentFaker();

            Assert.AreEqual(DocumentStatus.NoInvite, new SignNowDocument().Status);
            Assert.AreEqual(DocumentStatus.NoInvite, baseDocument.Generate().Status);

            // create free form invite request
            var documentWithOneRequest = baseDocument
                .RuleFor(
                    prop => prop.InviteRequests,
                    new FreeformInviteFaker().RuleFor(inv => inv.SignatureId, f => null).Generate(1))
                .Generate();

            // Freeform invite created assertions
            Assert.AreEqual(1, documentWithOneRequest.InviteRequests.Count);
            Assert.IsNull(documentWithOneRequest.InviteRequests[0].SignatureId, "Signature Id should be null for not signed document");
            Assert.AreEqual(InviteStatus.Pending, documentWithOneRequest.InviteRequests[0].Status);
            Assert.AreEqual(DocumentStatus.Pending, documentWithOneRequest.Status);


            // create free form invite request and sign the document
            var signerEmail = "signer@email.com";

            var documentWithOneSignedRequest = baseDocument
                .RuleFor(doc => doc.InviteRequests, f => new FreeformInviteFaker().Generate(1))
                .RuleFor(doc => doc.Signatures, f => new SignatureFaker().Generate(1))
                .FinishWith((f, obj) => {
                    var invite = obj.InviteRequests.GetEnumerator();
                    var sign = obj.Signatures.GetEnumerator();

                    while (invite.MoveNext() && sign.MoveNext())
                    {
                        invite.Current.SignatureId = sign.Current.Id;
                        invite.Current.SignerEmail = signerEmail;
                        invite.Current.Owner = obj.Owner;
                        sign.Current.Email = signerEmail;
                        sign.Current.UserId = invite.Current.UserId;
                        sign.Current.SignatureRequestId = invite.Current.Id;
                    }
                });

            var documentWithOneSign = documentWithOneSignedRequest.Generate();
            var actualDocumentSignature = documentWithOneSign.Signatures[0];
            var actualDocumentInvite = documentWithOneSign.InviteRequests[0];


            // asserts for document signed with only one freeform invite
            Assert.AreEqual(1, documentWithOneSign.InviteRequests.Count);
            Assert.AreEqual(1, documentWithOneSign.Signatures.Count);
            Assert.AreEqual(actualDocumentSignature.Id, actualDocumentInvite.SignatureId);
            Assert.AreEqual(actualDocumentSignature.UserId, actualDocumentInvite.UserId);
            Assert.AreEqual(actualDocumentSignature.SignatureRequestId, actualDocumentInvite.Id);
            Assert.AreEqual(actualDocumentSignature.Email, actualDocumentInvite.SignerEmail);
            Assert.AreEqual(InviteStatus.Fulfilled, actualDocumentInvite.Status);
            Assert.AreEqual(DocumentStatus.Completed, documentWithOneSign.Status);


            // create free form invite requests (one - signed, second - not signed yet)
            var documentWithTwoRequests = documentWithOneSignedRequest.Generate();
            var signer2Invite = new FreeformInviteFaker()
                    .RuleFor(inv => inv.Owner, documentWithTwoRequests.Owner)
                    .RuleFor(inv => inv.SignatureId, f => null)
                    .Generate();

            documentWithTwoRequests.InviteRequests.Add(signer2Invite);

            // asserts for document with two freeform invites (one - signed, second - not signed yet)
            Assert.AreEqual(2, documentWithTwoRequests.InviteRequests.Count);
            Assert.AreEqual(1, documentWithTwoRequests.Signatures.Count);
            Assert.IsTrue(documentWithTwoRequests.InviteRequests.TrueForAll(itm => itm.Owner == documentWithTwoRequests.Owner));
            Assert.IsNotNull(documentWithTwoRequests.InviteRequests[0].SignatureId);
            Assert.IsNull(documentWithTwoRequests.InviteRequests[1].SignatureId);
            Assert.AreEqual(InviteStatus.Fulfilled, documentWithTwoRequests.InviteRequests[0].Status);
            Assert.AreEqual(InviteStatus.Pending, documentWithTwoRequests.InviteRequests[1].Status);
            Assert.AreEqual(DocumentStatus.Pending, documentWithTwoRequests.Status);

            // sign second freeform invite and complete the document signing
            var documentWithTwoRequestsSigned = baseDocument
                .RuleFor(doc => doc.InviteRequests, f => new FreeformInviteFaker().Generate(2))
                .RuleFor(doc => doc.Signatures, f => new SignatureFaker().Generate(2))
                .FinishWith((f, obj) => {
                    var sig = obj.Signatures.GetEnumerator();

                    foreach (var inv in obj.InviteRequests)
                    {
                        sig.MoveNext();
                        inv.SignatureId = sig.Current.Id;
                        inv.UserId = sig.Current.UserId;
                        inv.SignerEmail = sig.Current.Email;
                        inv.Owner = obj.Owner;
                        inv.IsCanceled = false;
                    }
                })
                .Generate();

            // check if document fullfilled
            Assert.AreEqual(2, documentWithTwoRequestsSigned.Signatures.Count);
            Assert.IsTrue(documentWithTwoRequestsSigned.InviteRequests.TrueForAll(req => req.Status == InviteStatus.Fulfilled), "Invite requests should have a signature");
            Assert.AreEqual(DocumentStatus.Completed, documentWithTwoRequestsSigned.Status);
        }
    }
}
