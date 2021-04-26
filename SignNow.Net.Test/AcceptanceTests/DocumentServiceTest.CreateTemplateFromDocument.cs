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
            var templateName = "Template Name";
            var response =
                await SignNowTestContext.Documents.CreateTemplateFromDocumentAsync(TestPdfDocumentId, templateName);
            Assert.IsNotNull(response.Id);
            DisposableDocumentId = response.Id;
            var template = await SignNowTestContext.Documents.GetDocumentAsync(response.Id);
            Assert.AreEqual(templateName, template.Name);
            Assert.IsTrue(template.IsTemplate);
        }
    }
}
