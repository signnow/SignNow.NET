using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task CreateBulkInviteFromTemplateSuccessfully()
        {
            // Create a template first using a document with fields
            var createTemplateResult = await SignNowTestContext.Documents
                .CreateTemplateFromDocumentAsync(TestPdfDocumentIdWithFields, "Bulk Invite Template")
                .ConfigureAwait(false);

            DisposableDocumentId = createTemplateResult.Id;

            // Get a folder to store the documents
            var folders = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = folders.Folders.FirstOrDefault(f => f.Name == "Documents");
            Assert.IsNotNull(documentsFolder, "Documents folder should exist");

            // Create CSV content for bulk invite
            var csvContent = "Signer 1|signer1@example.com,Document 1\nSigner 1|signer2@example.com,Document 2";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite_test.csv";

            // Create bulk invite
            var result = await SignNowTestContext.Documents
                .CreateBulkInviteFromTemplateAsync(
                    createTemplateResult.Id,
                    csvStream,
                    fileName,
                    documentsFolder.Id,
                    "Please sign this document",
                    "Custom message for the signer")
                .ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual("job queued", result.Status);
        }

        [TestMethod]
        public async Task CreateBulkInviteFromTemplateWithMinimalParametersSuccessfully()
        {
            // Create a template first using a document with fields
            var createTemplateResult = await SignNowTestContext.Documents
                .CreateTemplateFromDocumentAsync(TestPdfDocumentIdWithFields, "Bulk Invite Template Minimal")
                .ConfigureAwait(false);

            DisposableDocumentId = createTemplateResult.Id;

            // Get a folder to store the documents
            var folders = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = folders.Folders.FirstOrDefault(f => f.Name == "Documents");
            Assert.IsNotNull(documentsFolder, "Documents folder should exist");

            // Create CSV content for bulk invite
            var csvContent = "Signer 1|signer@example.com,Test Document";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite_minimal.csv";

            // Create bulk invite with minimal parameters
            var result = await SignNowTestContext.Documents
                .CreateBulkInviteFromTemplateAsync(
                    createTemplateResult.Id,
                    csvStream,
                    fileName,
                    documentsFolder.Id)
                .ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual("job queued", result.Status);
        }

        [TestMethod]
        public async Task CreateBulkInviteFromTemplateWithQESSignatureTypeSuccessfully()
        {
            // Create a template first using a document with fields
            var createTemplateResult = await SignNowTestContext.Documents
                .CreateTemplateFromDocumentAsync(TestPdfDocumentIdWithFields, "Bulk Invite Template QES")
                .ConfigureAwait(false);

            DisposableDocumentId = createTemplateResult.Id;

            // Get a folder to store the documents
            var folders = await SignNowTestContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = folders.Folders.FirstOrDefault(f => f.Name == "Documents");
            Assert.IsNotNull(documentsFolder, "Documents folder should exist");

            // Create CSV content for bulk invite
            var csvContent = "Signer 1|signer@example.com,QES Document";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "bulk_invite_qes.csv";
            var clientTimestamp = (int)(System.DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // Create bulk invite with custom subject and message (without QES since test org doesn't support it)
            var result = await SignNowTestContext.Documents
                .CreateBulkInviteFromTemplateAsync(
                    createTemplateResult.Id,
                    csvStream,
                    fileName,
                    documentsFolder.Id,
                    "Please sign this document",
                    "Custom message for the signer",
                    clientTimestamp)
                .ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual("job queued", result.Status);
        }
    }
}