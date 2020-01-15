using System;
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
        /// <summary>
        /// Uploaded test document identity which will be deleted after run TestCase.
        /// </summary>
        private string DocumentId { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteDocument(DocumentId);
        }

        [TestMethod]
        public void DocumentOwnerCanSendFreeFormInviteToUser()
        {
            var signNow = new SignNowContext(Token);

            var invitee = new User
            {
                Email = "signnow.tutorial+test@gignnow.com",
                FirstName = "Alex",
                LastName = "Dou",
                Active = true
            };
            var invite = new FreeFormInvite(invitee.Email);

            DocumentId = UploadTestDocument(PdfFilePath, signNow.Documents);

            var inviteResponse = signNow.Invites.CreateInviteAsync(DocumentId, invite);
            Task.WaitAll(inviteResponse);

            Assert.IsFalse(inviteResponse.IsFaulted);
            Assert.AreEqual(invite.Recipient, invitee.Email);
            Assert.AreEqual(inviteResponse.Result.Id, inviteResponse.Result.Id.ValidateDocumentId());
        }
    }
}
