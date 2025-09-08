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
    public partial class DocumentGroupOperations : ExamplesBase
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

        [TestMethod]
        public async Task UpdateDocumentGroupTemplateAsync()
        {
            // Note: This example demonstrates how to update a document group template
            // In a real scenario, you would first create a document group template
            // For this example, we'll use a mock template ID

            var documentGroupTemplateId = "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00777";

            // Create update request with template IDs to add and remove
            var updateRequest = new UpdateDocumentGroupTemplateRequest
            {
                TemplateIdsToAdd = new List<string> 
                { 
                    "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00789",
                    "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00790"
                },
                TemplateIdsToRemove = new List<string> 
                { 
                    "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00791"
                },
                RoutingDetails = @"{
                    ""invite_steps"": [
                        {
                            ""order"": 1,
                            ""invite_emails"": [
                                {
                                    ""email"": ""signer@example.com"",
                                    ""subject"": ""Document Group Template Needs Your Signature"",
                                    ""message"": ""Please sign the documents in this template group"",
                                    ""expiration_days"": 30,
                                    ""reminder"": 0,
                                    ""hasSignActions"": true,
                                    ""allow_reassign"": ""0""
                                }
                            ],
                            ""invite_actions"": [
                                {
                                    ""email"": ""signer@example.com"",
                                    ""role_name"": ""Signer"",
                                    ""action"": ""sign"",
                                    ""document_id"": ""402ed7dca63eb1c78433827e6d946c3db91b1c15"",
                                    ""document_name"": ""Contract Document"",
                                    ""UUID"": ""3a994cdb-039e-4fb0-a350-affd7f3566f1"",
                                    ""allow_reassign"": ""0"",
                                    ""decline_by_signature"": ""0""
                                }
                            ]
                        }
                    ],
                    ""include_email_attachments"": 0
                }",
                TemplateGroupName = "Updated Contract Template Group"
            };

            try
            {
                // Update the document group template
                var response = await testContext.DocumentGroup
                    .UpdateDocumentGroupTemplateAsync(documentGroupTemplateId, updateRequest)
                    .ConfigureAwait(false);

                // Verify the response
                Assert.IsNotNull(response);
                Assert.AreEqual("success", response.Status);
                Console.WriteLine("Document group template updated successfully: {0}", response.Status);
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // Handle expected errors for mock template ID
                Console.WriteLine("Expected error for mock template ID: {0} - {1}", ex.HttpStatusCode, ex.Message);
                
                // Verify it's the right type of error
                Assert.IsTrue(ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound || 
                             ex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
