using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void CreateDocumentFromTemplateSuccessfully()
        {
            var templateId = SignNowTestContext.Documents
                .CreateTemplateFromDocumentAsync(TestPdfDocumentId, "test").Result.Id;
            var documentId = SignNowTestContext.Documents
                .CreateDocumentFromTemplateAsync(templateId, "test document name").Result.Id;
            Assert.IsNotNull(documentId);
        }
    }
}
