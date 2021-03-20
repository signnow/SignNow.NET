using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using UnitTests;

namespace FeatureTests
{
    [TestClass]
    public class EmbeddedSigningTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void CreatesEmbeddedSigningInviteForDocument()
        {
            DisposableDocumentId = UploadTestDocumentWithFieldExtract(
                PdfFilePath, "CreatesEmbeddedSigningInviteForDocument.pdf");
            var document = SignNowTestContext.Documents.GetDocumentAsync(DisposableDocumentId).Result;

            var invite = new EmbeddedSigningInvite(document);
            invite.AddEmbeddedSigningInvite(
                new EmbeddedInvite
                {
                    Email = "test.email+embedded@gmail.com",
                    RoleId = document.Roles[0].Id,
                    SigningOrder = 1
                });

            var embeddedSigningInvite = SignNowTestContext.Invites
                .CreateInviteAsync(document.Id, invite)
                .Result;

            TestUtils.Dump(embeddedSigningInvite);

            Assert.IsNotNull(embeddedSigningInvite);
            Assert.AreEqual(1, embeddedSigningInvite.InviteData.Count);
        }
    }
}
