using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task GetTheDocumentHistoryAsync()
        {
            // upload a document with a signature field
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "GetTheDocumentHistory.pdf")
                .ConfigureAwait(false);

            // get the document history
            var documentHistory = await testContext.Documents
                .GetDocumentHistoryAsync(document?.Id)
                .ConfigureAwait(false);

            // check the document history
            Assert.IsTrue(documentHistory.All(item => item.DocumentId == document?.Id));
            Assert.IsTrue(documentHistory.Any(item => item.Origin == "original"));
            Assert.IsTrue(documentHistory.All(item => item.Email == credentials.Login));

            // Clean up
            DeleteTestDocument(document?.Id);
        }
    }
}
