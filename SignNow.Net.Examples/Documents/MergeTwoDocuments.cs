using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task MergeTwoDocumentsAsync()
        {
            // upload two documents
            await using var file1Stream = File.OpenRead(PdfWithSignatureField);
            await using var file2Stream = File.OpenRead(PdfWithoutFields);

            var doc1 = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(file1Stream, "MergeTwoDocumentsTest1.pdf")
                .ConfigureAwait(false);
            var doc2 = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(file2Stream, "MergeTwoDocumentsTest2.pdf")
                .ConfigureAwait(false);

            // get uploaded documents
            var document1 = await testContext.Documents.GetDocumentAsync(doc1.Id).ConfigureAwait(false);
            var document2 = await testContext.Documents.GetDocumentAsync(doc2.Id).ConfigureAwait(false);

            // merge two documents
            var documents = new List<SignNowDocument> { document1, document2 };
            var finalDocument = await testContext.Documents
                .MergeDocumentsAsync("MergeTwoDocumentsTestResult.pdf", documents)
                .ConfigureAwait(false);

            // check merged document
            Assert.AreEqual("MergeTwoDocumentsTestResult.pdf", finalDocument.Filename);
            Assert.IsInstanceOfType(finalDocument.Document, typeof(Stream) );

            // clean up
            await testContext.Documents.DeleteDocumentAsync(doc1.Id);
            await testContext.Documents.DeleteDocumentAsync(doc2.Id);
        }
    }
}
