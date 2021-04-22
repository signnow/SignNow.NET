using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void CreateTemplateFromDocumentSuccessfully()
        {
            var response =
                SignNowTestContext.Documents.CreateTemplateFromDocumentAsync(TestPdfDocumentId, "test template name").Result;
            Assert.IsNotNull(response.Id);
        }
    }
}
