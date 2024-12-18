using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task UploadDocumentAsync()
        {
            await using var fileStream = File.OpenRead(PdfWithoutFields);

            // Upload the document
            var uploadResponse = await testContext.Documents
                .UploadDocumentAsync(fileStream, "document-example.pdf")
                .ConfigureAwait(false);

            // Gets document after from successful upload
            var uploadedDocument = await testContext.Documents
                .GetDocumentAsync(uploadResponse.Id)
                .ConfigureAwait(false);

            // Check uploaded document
            Assert.AreEqual(uploadResponse.Id, uploadedDocument.Id);

            // clean up
            DeleteTestDocument(uploadedDocument.Id);
        }
    }
}
