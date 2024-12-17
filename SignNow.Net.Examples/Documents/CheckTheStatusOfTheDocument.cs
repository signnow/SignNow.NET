using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task CheckTheStatusOfTheDocumentAsync()
        {
            // upload a document with a signature field
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "CheckTheStatusOfTheDocument.pdf")
                .ConfigureAwait(false);

            // get the document
            var documentStatus = await testContext.Documents
                .GetDocumentAsync(document?.Id)
                .ConfigureAwait(false);

            // check the status of the document
            Assert.AreEqual(DocumentStatus.NoInvite, documentStatus.Status);

            // delete the test document
            DeleteTestDocument(document?.Id);
        }
    }
}
