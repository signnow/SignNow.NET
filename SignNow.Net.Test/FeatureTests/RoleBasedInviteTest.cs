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

            var document = signNow.Documents.GetDocumentAsync(DocumentId).Result;
            var invite = new RoleBasedInvite(document);

            Assert.AreEqual(1, invite.DocumentRoles().Count);
            Assert.AreEqual("Signer 1", invite.DocumentRoles().First().Name);

            invite.AddRoleBasedInvite("signer1@signnow.com", invite.DocumentRoles().First());

            var inviteResponse = signNow.Invites.CreateInviteAsync(DocumentId, invite).Result;
            Assert.IsNull(inviteResponse.Id,"Successful Role-Based invite response doesnt contains Invite ID.");
        }
    }
}
