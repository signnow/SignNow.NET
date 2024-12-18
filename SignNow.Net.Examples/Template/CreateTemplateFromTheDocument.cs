using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class TemplateExamples
    {
        [TestMethod]
        public async Task CreateTemplateFromDocumentAsync()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            // Upload a document with a signature field
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentWithSignatureTextTag.pdf")
                .ConfigureAwait(false);

            // create a template from the uploaded document
            const string templateName = "Template Name";
            var result = await testContext.Documents
                .CreateTemplateFromDocumentAsync(document?.Id, templateName)
                .ConfigureAwait(false);

            // Get the new template created from the document
            var template = await testContext.Documents
                .GetDocumentAsync(result.Id)
                .ConfigureAwait(false);

            // Check that the document is a template
            Assert.IsNotNull(template?.Id);
            Assert.AreEqual(templateName, template.Name);
            Assert.IsTrue(template.IsTemplate);

            // clean up
            await testContext.Documents.DeleteDocumentAsync(template.Id).ConfigureAwait(false);
            DeleteTestDocument(document?.Id);
        }
    }
}
