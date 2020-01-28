using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.Constants;
using SignNow.Net.Test.Extensions;

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

            var invitee = new User
            {
                Email = "signnow.tutorial+test@signnow.com",
                FirstName = "Alex",
                LastName = "Dou",
                Active = true
            };
            var invite = new FreeFormInvite(invitee.Email);

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
            Assert.AreEqual(invitee.Email, documentInviteRequest.Signer, "Invite should contains user email whom was sent invite request.");
            Assert.IsNull(documentInviteRequest.IsCanceled, "Invite status should not be canceled by default.");
        }

        [TestMethod]
        public void ShouldGetFreeFormInviteSignStatus()
        {
            var mockDocument = JsonFixtures.DocumentTemplate.AsJsonObject();

            // successful freeform invite should have invite request inside the document
            var inviteRequests = (JArray)mockDocument["requests"];
            inviteRequests.Add(JsonFixtures.FreeFormInviteTemplate.AsJsonObject());
            inviteRequests[0]["signature_id"] = null;

            var documentWithRequest = JsonConvert.DeserializeObject<SignNowDocument>(mockDocument.ToString());

            // Freeform invite created assertions
            Assert.AreEqual(1, documentWithRequest.InviteRequests.Count);
            Assert.AreEqual("test.user@signnow.com", documentWithRequest.InviteRequests[0].Owner);
            Assert.AreEqual("signer@signnow.com", documentWithRequest.InviteRequests[0].Signer);
            Assert.IsNull(documentWithRequest.InviteRequests[0].SignatureId);
            Assert.IsFalse(documentWithRequest.IsFreeformInviteSigned());

            // Add signature by signing the document
            inviteRequests.RemoveAll();
            inviteRequests.Add(JsonFixtures.FreeFormInviteTemplate.AsJsonObject());
            inviteRequests[0]["signature_id"] = "signatureId00000000000000000000000SIGNED";

            var signature = (JArray)mockDocument["signatures"];
            signature.Add(JsonFixtures.SignatureTemplate.AsJsonObject());
            signature[0]["id"] = inviteRequests[0]["signature_id"];
            signature[0]["signature_request_id"] = inviteRequests[0]["unique_id"];

            var documentSigned = JsonConvert.DeserializeObject<SignNowDocument>(mockDocument.ToString());
            var actualSignature = documentSigned.Signatures[0];
            var actualInvite = documentSigned.InviteRequests[0];

            Assert.AreEqual(1, documentSigned.InviteRequests.Count);
            Assert.AreEqual(1, documentSigned.Signatures.Count);
            Assert.AreEqual(actualSignature.Id, actualInvite.SignatureId);
            Assert.AreEqual(actualSignature.UserId, actualInvite.UserId);
            Assert.AreEqual(actualSignature.SignatureRequestId, actualInvite.Id);
            Assert.AreEqual(actualSignature.Email, actualInvite.Signer);
            Assert.IsTrue(documentSigned.IsFreeformInviteSigned());
        }
    }
}
