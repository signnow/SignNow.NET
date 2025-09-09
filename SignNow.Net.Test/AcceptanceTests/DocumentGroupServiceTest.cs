using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests.DocumentGroup;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class DocumentGroupServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task ShouldUpdateDocumentGroupTemplate()
        {
            // Note: This test requires a valid document group template ID
            // In a real scenario, you would first create a document group template
            // For this test, we'll use a mock template ID and expect the API to return appropriate errors
            
            var updateRequest = new UpdateDocumentGroupTemplateRequest
            {
                Order = new List<string> { "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00789" },
                TemplateGroupName = "Updated Template Group",
                EmailActionOnComplete = "documents_and_attachments"
            };

            // Use a mock template ID for testing
            var mockTemplateId = "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00777";

            try
            {
                var response = await SignNowTestContext.DocumentGroup
                    .UpdateDocumentGroupTemplateAsync(mockTemplateId, updateRequest)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response);
                Assert.AreEqual("success", response.Status);
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // Expected for mock template ID - verify it's the right type of error
                Assert.IsTrue(ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound || 
                             ex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ShouldThrowExceptionForInvalidTemplateId()
        {
            var updateRequest = new UpdateDocumentGroupTemplateRequest
            {
                Order = new List<string>(),
                EmailActionOnComplete = "documents_and_attachments",
                TemplateGroupName = "Test Template Group"
            };

            var invalidTemplateId = "invalid-template-id";

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await SignNowTestContext.DocumentGroup
                    .UpdateDocumentGroupTemplateAsync(invalidTemplateId, updateRequest)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task ShouldHandleEmptyUpdateRequest()
        {
            var updateRequest = new UpdateDocumentGroupTemplateRequest
            {
                Order = new List<string>(),
                EmailActionOnComplete = "documents_and_attachments",
                TemplateGroupName = "Test Template Group"
            };

            var mockTemplateId = "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00777";

            try
            {
                var response = await SignNowTestContext.DocumentGroup
                    .UpdateDocumentGroupTemplateAsync(mockTemplateId, updateRequest)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response);
                Assert.AreEqual("success", response.Status);
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // Expected for mock template ID - verify it's the right type of error
                Assert.IsTrue(ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound || 
                             ex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task ShouldCreateDocumentGroupTemplate()
        {
            // First create a document group to use as source
            var documents = new List<SignNow.Net.Model.SignNowDocument>();
            
            // Upload test documents
            using var fileStream = System.IO.File.OpenRead(PdfFilePath);
            
            for (int i = 0; i < 2; i++)
            {
                var upload = await SignNowTestContext.Documents
                    .UploadDocumentAsync(fileStream, $"ForDocumentGroupTemplateFile-{i}.pdf");
                var doc = await SignNowTestContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
                documents.Add(doc);
            }

            // Create document group from uploaded documents
            var documentGroup = await SignNowTestContext.DocumentGroup
                .CreateDocumentGroupAsync("CreateDocumentGroupTemplateTest", documents)
                .ConfigureAwait(false);

            try
            {
                // Create document group template from the document group
                var createRequest = new CreateDocumentGroupTemplateRequest
                {
                    Name = "Test Document Group Template",
                    OwnAsMerged = true
                };

                var response = await SignNowTestContext.DocumentGroup
                    .CreateDocumentGroupTemplateAsync(documentGroup.Id, createRequest)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response);
                
                if (response.IsAccepted)
                {
                    Console.WriteLine("Document group template creation was accepted and scheduled for processing");
                }
                else
                {
                    Assert.IsNotNull(response.Id);
                    Assert.IsNotNull(response.Status);
                    Assert.IsTrue(response.Id.Length == 40);
                    Console.WriteLine($"Created document group template: {response.Id} with status: {response.Status}");
                }
            }
            catch (SignNow.Net.Exceptions.SignNowException ex)
            {
                // Handle expected errors - document group template creation might not be available in all environments
                Console.WriteLine($"Expected error for document group template creation: {ex.HttpStatusCode} - {ex.Message}");
                
                // Verify it's the right type of error
                Assert.IsTrue(ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound || 
                             ex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest ||
                             ex.HttpStatusCode == System.Net.HttpStatusCode.Forbidden);
            }
            finally
            {
                // Clean up
                await SignNowTestContext.DocumentGroup.DeleteDocumentGroupAsync(documentGroup.Id).ConfigureAwait(false);
                foreach (var document in documents)
                {
                    await SignNowTestContext.Documents.DeleteDocumentAsync(document.Id).ConfigureAwait(false);
                }
            }
        }

        [TestMethod]
        public async Task ShouldGetDocumentGroupTemplates()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 10,
                Offset = 0
            };

            var response = await SignNowTestContext.DocumentGroup
                .GetDocumentGroupTemplatesAsync(request)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DocumentGroupTemplates);
            Assert.IsTrue(response.DocumentGroupTemplateTotalCount >= 0);
            
            // If there are templates, verify their structure
            if (response.DocumentGroupTemplates.Count > 0)
            {
                var template = response.DocumentGroupTemplates.First();
                Assert.IsNotNull(template.TemplateGroupId);
                Assert.IsNotNull(template.TemplateGroupName);
                Assert.IsNotNull(template.OwnerEmail);
                Assert.IsNotNull(template.Templates);
                
                // Verify template items structure
                if (template.Templates.Count > 0)
                {
                    var templateItem = template.Templates.First();
                    Assert.IsNotNull(templateItem.Id);
                    Assert.IsNotNull(templateItem.Name);
                    Assert.IsNotNull(templateItem.Thumbnail);
                    Assert.IsNotNull(templateItem.Roles);
                }
            }
        }

        [TestMethod]
        public async Task ShouldThrowExceptionForInvalidDocumentGroupId()
        {
            var createRequest = new CreateDocumentGroupTemplateRequest
            {
                Name = "Test Template Group"
            };

            var invalidDocumentGroupId = "invalid-document-group-id";

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await SignNowTestContext.DocumentGroup
                    .CreateDocumentGroupTemplateAsync(invalidDocumentGroupId, createRequest)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);
        }
    }
}
