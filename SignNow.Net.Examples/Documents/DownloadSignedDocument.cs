using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task DownloadSignedDocumentAsync()
        {
            // Upload document
            await using var fileStream = File.OpenRead(PdfWithoutFields);
            var document = await testContext.Documents
                .UploadDocumentAsync(fileStream, "SignedDocumentTest.pdf")
                .ConfigureAwait(false);

            // Download signed document
            var documentSigned = await testContext.Documents
                .DownloadDocumentAsync(document.Id, DownloadType.PdfCollapsed)
                .ConfigureAwait(false);

            // Check if document is downloaded
            Assert.AreEqual("SignedDocumentTest.pdf", documentSigned.Filename);
            Assert.IsInstanceOfType(documentSigned.Document, typeof(Stream));

            // Clean up
            DeleteTestDocument(document.Id);
        }
    }
}
