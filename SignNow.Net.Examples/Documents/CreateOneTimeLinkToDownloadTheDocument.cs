using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task CreateOneTimeLinkToDownloadTheDocumentAsync()
        {
            // Upload a document with a signature field
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CreateOneTimeLinkToDownloadTheDocumentTest.pdf")
                .ConfigureAwait(false);

            // Create a one-time use URL for anyone to download the document as a PDF
            var oneTimeLink = await testContext.Documents
                .CreateOneTimeDownloadLinkAsync(document?.Id)
                .ConfigureAwait(false);

            // Check if the URL is valid
            StringAssert.Contains(oneTimeLink.Url.Host, "signnow.com");

            // Clean up
            DeleteTestDocument(document?.Id);
        }
    }
}
