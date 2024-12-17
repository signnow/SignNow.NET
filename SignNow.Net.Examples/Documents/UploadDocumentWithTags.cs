using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;

namespace SignNow.Net.Examples
{
    [TestClass]
    public partial class DocumentExamples : ExamplesRunner
    {
        [TestMethod]
        public async Task UploadDocumentWithTagsAsync()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            // Upload the document with field extract
            var documentWithFields = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentSampleWithSignatureTextTag.pdf")
                .ConfigureAwait(false);

            // Get uploaded document
            var uploadedDocument = await testContext.Documents
                .GetDocumentAsync(documentWithFields.Id)
                .ConfigureAwait(false);

            // Check that document has fields
            using var documentFields = uploadedDocument?.Fields.GetEnumerator();
            documentFields?.MoveNext();

            Assert.AreEqual(FieldType.Text, documentFields?.Current?.Type);
            Assert.IsTrue(uploadedDocument?.Fields.Count > 0);

            // clean up
            DeleteTestDocument(uploadedDocument.Id);
        }
    }
}
