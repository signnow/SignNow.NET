using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.EditFields;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task PrefillTextFieldsAsync()
        {
            // Upload test document
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "PrefillDocumentTest.pdf")
                .ConfigureAwait(false);

            // get uploaded document
            var documentUploaded = await testContext.Documents.GetDocumentAsync(testDocument.Id).ConfigureAwait(false);
            Assert.IsNull(documentUploaded.Fields.FirstOrDefault()?.JsonAttributes.PrefilledText);

            // Add simple text field which will be prefilled next.
            var editFields = new List<IFieldEditable>
            {
                new TextField
                {
                    PageNumber = 0,
                    Name = "Text_1",
                    Height = 40,
                    Width = 200,
                    X = 10,
                    Y = 40,
                    Role = "Signer 1"
                }
            };

            var editDocument = await testContext.Documents
                .EditDocumentAsync(testDocument.Id, editFields)
                .ConfigureAwait(false);

            // Get edited document
            var documentEdited = await testContext.Documents.GetDocumentAsync(editDocument.Id).ConfigureAwait(false);

            // Check that document has fields
            Assert.IsNull(documentEdited.Fields.FirstOrDefault()?.JsonAttributes.PrefilledText);
            Assert.AreEqual("Text_1", documentEdited.Fields.FirstOrDefault()?.JsonAttributes.Name);

            // Prefill text field
            var fields = new List<TextField>
            {
                new TextField
                {
                    Name = "Text_1",
                    PrefilledText = "Test Prefill"
                }
            };

            await testContext.Documents.PrefillTextFieldsAsync(testDocument.Id, fields).ConfigureAwait(false);

            // Get final document
            var documentFinal = await testContext.Documents.GetDocumentAsync(testDocument.Id).ConfigureAwait(false);

            // Check that document has fields
            Assert.AreEqual("Test Prefill", documentFinal.Fields.FirstOrDefault()?.JsonAttributes.PrefilledText);

            // clean up
            DeleteTestDocument(documentUploaded.Id);
            DeleteTestDocument(documentEdited.Id);
            DeleteTestDocument(documentFinal.Id);
        }
    }
}
