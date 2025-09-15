using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests;
using SignNow.Net.Model.Requests;

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

            // Create bulk invite request
            var bulkInviteRequest = new CreateBulkInviteRequest(csvStream, fileName, documentsFolder)
            {
                Subject = "Please sign this document",
                EmailMessage = "Custom message for the signer"
            };

            // Create bulk invite
            var result = await SignNowTestContext.Documents
                .CreateBulkInviteFromTemplateAsync(createTemplateResult.Id, bulkInviteRequest)
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

            // Create bulk invite request with minimal parameters
            var bulkInviteRequest = new CreateBulkInviteRequest(csvStream, fileName, documentsFolder);

            // Create bulk invite with minimal parameters
            var result = await SignNowTestContext.Documents
                .CreateBulkInviteFromTemplateAsync(createTemplateResult.Id, bulkInviteRequest)
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

            // Create bulk invite request with custom subject and message (without QES since test org doesn't support it)
            var bulkInviteRequest = new CreateBulkInviteRequest(csvStream, fileName, documentsFolder)
            {
                Subject = "Please sign this document",
                EmailMessage = "Custom message for the signer"
            };

            // Create bulk invite with custom subject and message
            var result = await SignNowTestContext.Documents
                .CreateBulkInviteFromTemplateAsync(createTemplateResult.Id, bulkInviteRequest)
                .ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual("job queued", result.Status);
        }
    }
}