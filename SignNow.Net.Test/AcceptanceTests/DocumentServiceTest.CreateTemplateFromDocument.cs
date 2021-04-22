using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task CreateTemplateFromDocumentSuccessfully()
        {
            var response =
                await SignNowTestContext.Documents.CreateTemplateFromDocumentAsync(new CreateTemplateFromDocumentRequest
                {
                    DocumentId = TestPdfDocumentId,
                    TemplateName = "test template name"
                });

            Assert.IsNotNull(response.Id);
        }
    }
}
