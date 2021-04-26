using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task CreateTemplateFromDocumentSuccessfully()
        {
            var templateName = "Template Name";
            var response = await SignNowTestContext.Documents
                .CreateTemplateFromDocumentAsync(TestPdfDocumentId, templateName)
                .ConfigureAwait(false);

            Assert.IsNotNull(response.Id);
            DisposableDocumentId = response.Id;

            var template = await SignNowTestContext.Documents
                .GetDocumentAsync(response.Id)
                .ConfigureAwait(false);

            Assert.AreEqual(templateName, template.Name);
            Assert.IsTrue(template.IsTemplate);
        }
    }
}
