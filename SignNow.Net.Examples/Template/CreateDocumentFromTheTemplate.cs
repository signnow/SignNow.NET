using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    [TestClass]
    public partial class TemplateExamples: ExamplesBase
    {
        [TestMethod]
        public async Task CreateDocumentFromTemplateAsync()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            // Upload a document with a signature field
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentWithSignatureTextTag.pdf")
                .ConfigureAwait(false);

            // Create a template from the uploaded document
            var template = await testContext.Documents
                .CreateTemplateFromDocumentAsync(testDocument.Id, "TemplateName")
                .ConfigureAwait(false);

            // Creates a new document copy out of template
            var documentName = "Document Name";
            var result = await testContext.Documents
                .CreateDocumentFromTemplateAsync(template.Id, documentName)
                .ConfigureAwait(false);

            // Get the new document created from template
            var document = await testContext.Documents
                .GetDocumentAsync(result.Id)
                .ConfigureAwait(false);

            // Check that the document is not a template
            Assert.IsNotNull(document?.Id);
            Assert.IsFalse(document.IsTemplate);
            Assert.AreEqual(documentName, document.Name);

            // clean up
            await testContext.Documents.DeleteDocumentAsync(document.Id).ConfigureAwait(false);
            await testContext.Documents.DeleteDocumentAsync(template.Id).ConfigureAwait(false);
            DeleteTestDocument(testDocument?.Id);
        }
    }
}
