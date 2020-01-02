using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void ShouldCreateSignInvite()
        {
            var user = new UserService(Token);
            var sender = user.GetCurrentUserAsync().Result;
            var recipient = "signnow.tutorial+test@gmail.com";

            DocumentId = UploadTestDocument(PdfFilePath);

            var invite = new FreeFormInvite
            {
                Sender = sender,
                Recipient = recipient
            };

            var expectedContent = $"{{\"from\":\"{sender.Email}\",\"to\":\"{recipient}\"}}";
            var inviteResponse = docService.CreateInviteAsync(DocumentId, invite).Result;

            Assert.AreSame(sender.Email, invite.Sender.Email);
            Assert.AreEqual(expectedContent, invite.InviteContent().GetHttpContent().ReadAsStringAsync().Result);
            Assert.IsNotNull(inviteResponse.Id);
        }
    }
}
