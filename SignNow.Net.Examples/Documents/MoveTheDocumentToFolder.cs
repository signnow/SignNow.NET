using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task MoveDocumentToFolderAsync()
        {
            // Upload test document
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var testDocument = await testContext.Documents
                .UploadDocumentAsync(fileStream, "MoveDocumentTest.pdf");

            // Create new Folder where you'd like to keep test document
            var root = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = root.Folders.FirstOrDefault(f => f.Name == "Documents");
            var folderToMove = await testContext.Folders
                .CreateFolderAsync("FolderToMoveDocument", documentsFolder?.Id)
                .ConfigureAwait(false);

            // Move test document to folder created with previous step
            await testContext.Documents
                .MoveDocumentAsync(testDocument.Id, folderToMove.Id)
                .ConfigureAwait(false);

            // Get folder with updated document
            var folderToMoveUpdated = await testContext.Folders
                .GetFolderAsync(folderToMove.Id)
                .ConfigureAwait(false);

            // Check if test document has been moved
            Assert.AreEqual("FolderToMoveDocument", folderToMoveUpdated.Name);
            Assert.AreEqual(1, folderToMoveUpdated.TotalDocuments);
            Assert.AreEqual(testDocument.Id, folderToMoveUpdated.Documents.FirstOrDefault(d => d.Id == testDocument.Id)?.Id);

            // Finally - delete document and folder
            await testContext.Documents.DeleteDocumentAsync(testDocument.Id).ConfigureAwait(false);
            await testContext.Folders.DeleteFolderAsync(folderToMoveUpdated.Id).ConfigureAwait(false);
        }
    }
}
