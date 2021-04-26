using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task CreateDocumentFromTemplateSuccessfully()
        {
            var createTemplateResult = await SignNowTestContext.Documents
                .CreateTemplateFromDocumentAsync(TestPdfDocumentId, "Template Name").ConfigureAwait(false);
            DisposableDocumentId = createTemplateResult.Id;
            var documentName = "test document name";
            var result = await SignNowTestContext.Documents
                .CreateDocumentFromTemplateAsync(createTemplateResult.Id, documentName).ConfigureAwait(false);
            Assert.IsNotNull(result?.Id);

            var document = await SignNowTestContext.Documents.GetDocumentAsync(result.Id).ConfigureAwait(false);
            Assert.IsNotNull(document.Id);
            Assert.AreEqual(documentName, document.Name);
            Assert.IsFalse(document.IsTemplate);

            await SignNowTestContext.Documents.DeleteDocumentAsync(document.Id).ConfigureAwait(false);
        }
    }
}
