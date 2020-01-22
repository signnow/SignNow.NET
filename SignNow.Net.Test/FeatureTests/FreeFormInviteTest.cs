using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Test;

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
            Assert.AreEqual(inviteResponse.Id, inviteResponse.Id.ValidateDocumentId(), "Successful invite response should contains valid Invite ID.");

            // Check if invite was successful and the document contains invite request data
            var documentInfo = signNow.Documents.GetDocumentAsync(DocumentId).Result;
            var inviteIdx = documentInfo.InviteRequests.FindIndex(request => request.Id == inviteResponse.Id);
            var documentInviteRequest = documentInfo.InviteRequests[inviteIdx];

            Assert.AreEqual(DocumentId, documentInfo.Id, "You should get proper document details.");
            Assert.AreEqual(inviteResponse.Id, documentInviteRequest.Id, "Document should contains freeform invite ID after invite has been sent.");
            Assert.AreEqual(invitee.Email, documentInviteRequest.Signer, "Invite should contains user email whom was sent invite request.");
            Assert.IsNull(documentInviteRequest.IsCanceled, "Invite status should not be canceled by default.");
        }
    }
}
