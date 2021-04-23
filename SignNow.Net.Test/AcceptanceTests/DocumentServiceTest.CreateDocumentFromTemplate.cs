using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        [DataRow("test document name")]
        public async Task CreateDocumentFromTemplateSuccessfully(string documentName)
        {
            var createTemplateRequest = new CreateTemplateFromDocumentRequest("Template Name", TestPdfDocumentId);
            var createTemplateResult = await SignNowTestContext.Documents.CreateTemplateFromDocumentAsync(createTemplateRequest);
            DisposableDocumentId = createTemplateResult.Id;

            var result = await SignNowTestContext.Documents.CreateDocumentFromTemplateAsync(createTemplateResult.Id, documentName);
            Assert.IsNotNull(result?.Id);

            var document = await SignNowTestContext.Documents.GetDocumentAsync(result.Id);
            Assert.AreEqual(documentName, document.Name);

            await SignNowTestContext.Documents.DeleteDocumentAsync(document.Id);
        }
    }
}
