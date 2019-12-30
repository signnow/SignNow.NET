using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void ShouldCreateSignInvite()
        {
            DocumentId = UploadTestDocument(PdfFilePath);

            var invite = new FreeFormInvite
            {
                Sender = "signnow.tutorial+dotnet@gmail.com",
                Recipient = "signnow.tutorial+dotnettest@gmail.com"
            };

            var expectedContent = "{\"from\":\"signnow.tutorial+dotnet@gmail.com\",\"to\":\"signnow.tutorial+dotnettest@gmail.com\"}";
            var inviteResponse = docService.CreateInviteAsync(DocumentId, invite).Result;

            Assert.AreSame("signnow.tutorial+dotnet@gmail.com", invite.Sender);
            Assert.AreSame("signnow.tutorial+dotnettest@gmail.com", invite.Recipient);
            Assert.AreEqual(expectedContent, invite.InviteContent().GetHttpContent().ReadAsStringAsync().Result);
            Assert.IsNotNull(inviteResponse.Id);
        }
    }
}