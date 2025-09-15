using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model;

namespace SignNow.Net.Examples
{
    public partial class TemplateExamples
    {
        [TestMethod]
        public async Task CreateBulkInviteFromTemplateAsync()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            // Upload a document with a signature field
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentWithSignatureTextTag.pdf")
                .ConfigureAwait(false);

            // Create a template from the uploaded document
            var template = await testContext.Documents
                .CreateTemplateFromDocumentAsync(testDocument.Id, "Bulk Invite Template")
                .ConfigureAwait(false);

            // Get a folder to store the documents
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = folders.Folders.FirstOrDefault(f => f.Name == "Documents");
            Assert.IsNotNull(documentsFolder, "Documents folder should exist");

            // Create CSV content for bulk invite
            // Format: Role|Email,DocumentName
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Signer 1|signer1@example.com,Contract for John Doe");
            csvContent.AppendLine("Signer 1|signer2@example.com,Contract for Jane Smith");
            csvContent.AppendLine("Signer 1|signer3@example.com,Contract for Bob Johnson");

            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent.ToString()));
            var fileName = "bulk_invite_contracts.csv";

            // Create bulk invite request
            var bulkInviteRequest = new CreateBulkInviteRequest(csvStream, fileName, documentsFolder)
            {
                Subject = "Please sign your contract",
                EmailMessage = "Thank you for choosing our services. Please review and sign your contract."
            };

            // Create bulk invite from template
            var result = await testContext.Documents
                .CreateBulkInviteFromTemplateAsync(template.Id, bulkInviteRequest)
                .ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual("job queued", result.Status);
        }

        [TestMethod]
        public async Task CreateBulkInviteFromTemplateWithMinimalParametersAsync()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            // Upload a document with a signature field
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentWithSignatureTextTag.pdf")
                .ConfigureAwait(false);

            // Create a template from the uploaded document
            var template = await testContext.Documents
                .CreateTemplateFromDocumentAsync(testDocument.Id, "Minimal Bulk Invite Template")
                .ConfigureAwait(false);

            // Get a folder to store the documents
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = folders.Folders.FirstOrDefault(f => f.Name == "Documents");
            Assert.IsNotNull(documentsFolder, "Documents folder should exist");

            // Create CSV content for bulk invite with minimal parameters
            var csvContent = "Signer 1|signer@example.com,Simple Document";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "minimal_bulk_invite.csv";

            // Create bulk invite request with only required parameters
            var bulkInviteRequest = new CreateBulkInviteRequest(csvStream, fileName, documentsFolder);

            // Create bulk invite with only required parameters
            var result = await testContext.Documents
                .CreateBulkInviteFromTemplateAsync(template.Id, bulkInviteRequest)
                .ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual("job queued", result.Status);
        }

        [TestMethod]
        public async Task CreateBulkInviteFromTemplateWithMultipleRolesAsync()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            // Upload a document with a signature field
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentWithSignatureTextTag.pdf")
                .ConfigureAwait(false);

            // Create a template from the uploaded document
            var template = await testContext.Documents
                .CreateTemplateFromDocumentAsync(testDocument.Id, "Multi-Role Bulk Invite Template")
                .ConfigureAwait(false);

            // Get a folder to store the documents
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = folders.Folders.FirstOrDefault(f => f.Name == "Documents");
            Assert.IsNotNull(documentsFolder, "Documents folder should exist");

            // Create CSV content for bulk invite with single role (matching the template)
            // Format: Role|Email,DocumentName
            // Note: The template was created from a document with one signature field, so we use one role
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Signer 1|signer1@example.com,Multi-Role Document 1");
            csvContent.AppendLine("Signer 1|signer2@example.com,Multi-Role Document 2");

            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent.ToString()));
            var fileName = "single_role_bulk_invite.csv";

            // Create bulk invite request with custom subject and message
            var bulkInviteRequest = new CreateBulkInviteRequest(csvStream, fileName, documentsFolder)
            {
                Subject = "Single-Role Document Signing",
                EmailMessage = "This document requires your signature. Please review and sign."
            };

            // Create bulk invite with custom subject and message
            var result = await testContext.Documents
                .CreateBulkInviteFromTemplateAsync(template.Id, bulkInviteRequest)
                .ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual("job queued", result.Status);
        }

        [TestMethod]
        public async Task CreateBulkInviteFromTemplateWithSignatureTypeAsync()
        {
            await using var fileStream = File.OpenRead(PdfWithSignatureField);

            // Upload a document with a signature field
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentWithSignatureTextTag.pdf")
                .ConfigureAwait(false);

            // Create a template from the uploaded document
            var template = await testContext.Documents
                .CreateTemplateFromDocumentAsync(testDocument.Id, "QES Bulk Invite Template")
                .ConfigureAwait(false);

            // Get a folder to store the documents
            var folders = await testContext.Folders.GetAllFoldersAsync().ConfigureAwait(false);
            var documentsFolder = folders.Folders.FirstOrDefault(f => f.Name == "Documents");
            Assert.IsNotNull(documentsFolder, "Documents folder should exist");

            // Create CSV content for bulk invite
            var csvContent = "Signer 1|signer@example.com,QES Document";
            var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var fileName = "qes_bulk_invite.csv";

            // Create bulk invite request with QES signature type
            var bulkInviteRequest = new CreateBulkInviteRequest(csvStream, fileName, documentsFolder)
            {
                Subject = "QES Document Signing",
                EmailMessage = "This document requires a qualified electronic signature.",
                SignatureType = SignatureType.Eideasy
            };

            // Create bulk invite with QES signature type
            var result = await testContext.Documents
                .CreateBulkInviteFromTemplateAsync(template.Id, bulkInviteRequest)
                .ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual("job queued", result.Status);
        }
    }
}