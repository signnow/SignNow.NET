using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;
using UnitTests;
using DownloadType = SignNow.Net.Model.Requests.DocumentGroup.DownloadType;

namespace SignNow.Net.Examples
{
    [TestClass]
    public partial class DocumentGroupOperations : ExamplesRunner
    {
        [TestMethod]
        public async Task BasicOperationsWithDocumentGroupAsync()
        {
            // Upload test documents
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            var documents = new List<SignNowDocument>();

            for (int i = 0; i < 2; i++)
            {
                var upload = await testContext.Documents
                    .UploadDocumentAsync(fileStream, $"ForDocumentGroupFile-{i}.pdf");
                var doc = await testContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
                documents.Add(doc);
            }

            // Create document group from uploaded documents
            var documentGroup = await testContext.DocumentGroup
                .CreateDocumentGroupAsync("CreateDocumentGroupTest", documents)
                .ConfigureAwait(false);

            // Get document group by id
            var createdDocumentGroup = await testContext.DocumentGroup.GetDocumentGroupInfoAsync(documentGroup.Id).ConfigureAwait(false);

            // Check if document group was created
            Assert.IsTrue(documentGroup.Id.Length == 40);
            Assert.AreEqual("CreateDocumentGroupTest", createdDocumentGroup.Data.Name);
            Assert.AreEqual(documents.Count, createdDocumentGroup.Data.Documents.Count);
            Console.WriteLine("Created document group: {0} with name {1}", documentGroup.Id, createdDocumentGroup.Data.Name);

            // rename document group
            await testContext.DocumentGroup.RenameDocumentGroupAsync("renamedDocumentGroup", documentGroup.Id).ConfigureAwait(false);

            // Get document group by id
            var renamedDocumentGroup = await testContext.DocumentGroup.GetDocumentGroupInfoAsync(documentGroup.Id).ConfigureAwait(false);

            // check if document group was renamed
            Assert.AreEqual("renamedDocumentGroup", renamedDocumentGroup.Data.Name);
            Console.WriteLine("Document group was renamed: {0} => {1}", createdDocumentGroup.Data.Name, renamedDocumentGroup.Data.Name);

            // Download document group files
            var pdfFiles = await testContext.DocumentGroup
                .DownloadDocumentGroupAsync(documentGroup.Id, new DownloadOptions {DownloadType = DownloadType.MergedPdf})
                .ConfigureAwait(false);
            var zipFile = await testContext.DocumentGroup
                .DownloadDocumentGroupAsync(documentGroup.Id, new DownloadOptions {DownloadType = DownloadType.Zip})
                .ConfigureAwait(false);

            // Check if downloaded files are PDF and ZIP
            Assert.That.StreamIsPdf(pdfFiles.Document);
            Assert.That.StreamIsZip(zipFile.Document);
            Assert.IsTrue(pdfFiles.Length > 0);
            Assert.IsTrue(zipFile.Length > 0);
            Assert.AreEqual("application/pdf", pdfFiles.MediaType);
            Assert.AreEqual("application/zip", zipFile.MediaType);
            Console.WriteLine("Document group downloades as: {0} with name {1} and size {2} bytes", pdfFiles.MediaType, pdfFiles.Filename, pdfFiles.Length);
            Console.WriteLine("Document group downloades as: {0} with name {1} and size {2} bytes", zipFile.MediaType, zipFile.Filename, zipFile.Length);

            // Clean up
            await testContext.DocumentGroup.DeleteDocumentGroupAsync(documentGroup.Id).ConfigureAwait(false);
            foreach (var document in documents)
            {
                DeleteTestDocument(document.Id);
            }
        }
    }
}
