using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void CreateTemplateFromDocumentSuccessfully()
        {
            var response =
                SignNowTestContext.Documents.CreateTemplateFromDocumentAsync(new CreateTemplateFromDocumentRequest
                {
                    DocumentId = TestPdfDocumentId,
                    TemplateName = "test template name"
                }).Result;

            Assert.IsNotNull(response.Id);
        }
    }
}
