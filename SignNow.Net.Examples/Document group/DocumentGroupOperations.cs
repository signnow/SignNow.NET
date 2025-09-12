using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            // This example demonstrates how to update a document group template.
            // Document group templates are different from regular document templates.
            // A document group template contains multiple document templates and defines
            // routing details for the signing process.
            
            // First, create a document group to use as source for the template
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            
            var documents = new List<SignNowDocument>();
            for (int i = 0; i < 2; i++)
            {
                var upload = await testContext.Documents
                    .UploadDocumentAsync(fileStream, $"ForDocumentGroupTemplateFile-{i}.pdf");
                var doc = await testContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
                documents.Add(doc);
            }

            // Create document group from uploaded documents
            var documentGroup = await testContext.DocumentGroup
                .CreateDocumentGroupAsync("CreateDocumentGroupTemplateTest", documents)
                .ConfigureAwait(false);

            // Verify the document group was created
            Assert.IsTrue(documentGroup.Id.Length == 40);
            Console.WriteLine("Created document group: {0}", documentGroup.Id);

            // Create a document group template from the document group
            var createTemplateRequest = new CreateDocumentGroupTemplateRequest
            {
                Name = "Contract Template Group",
                OwnAsMerged = true
            };

            // Note: This endpoint returns 202 Accepted with empty body for asynchronous processing
            var documentGroupTemplate = await testContext.DocumentGroup
                .CreateDocumentGroupTemplateAsync(documentGroup.Id, createTemplateRequest)
                .ConfigureAwait(false);

            // Now create some additional templates to use in the update operation
            var template1 = await testContext.Documents
                .CreateTemplateFromDocumentAsync(documents[0].Id, "Additional Template 1")
                .ConfigureAwait(false);
            var template2 = await testContext.Documents
                .CreateTemplateFromDocumentAsync(documents[1].Id, "Additional Template 2")
                .ConfigureAwait(false);

            // Verify the document group template response
            Assert.IsNotNull(documentGroupTemplate);
            
            if (documentGroupTemplate.IsAccepted)
            {
                // 202 Accepted response with empty body - operation was scheduled
                Console.WriteLine("Document group template creation was accepted and scheduled for processing");
                
                // Try to get an existing document group template to use for the update example
                var getTemplatesRequest = new GetDocumentGroupTemplatesRequest
                {
                    Limit = 10,
                    Offset = 0
                };
                
                var existingTemplates = await testContext.DocumentGroup
                    .GetDocumentGroupTemplatesAsync(getTemplatesRequest)
                    .ConfigureAwait(false);
                
                if (existingTemplates.DocumentGroupTemplates.Count > 0)
                {
                    var existingTemplate = existingTemplates.DocumentGroupTemplates.First();
                    Console.WriteLine("Using existing template ID for demonstration: {0}", existingTemplate.TemplateGroupId);
                
                       // Update the document group template using the existing template ID
                       // Use the actual document IDs from the existing template
                       var documentIds = existingTemplate.Templates?.Select(t => t.Id).ToList() ?? new List<string>();
                       
                       var updateRequest = new UpdateDocumentGroupTemplateRequest
                       {
                           Order = documentIds,
                           TemplateGroupName = "Updated Contract Template Group",
                           EmailActionOnComplete = EmailActionsType.DocumentsAndAttachments
                       };

                    // Update the document group template using the existing template ID
                    var response = await testContext.DocumentGroup
                        .UpdateDocumentGroupTemplateAsync(existingTemplate.TemplateGroupId, updateRequest)
                        .ConfigureAwait(false);

                    // Verify the response structure (PATCH returns 204 No Content, so response might be null)
                    if (response != null)
                    {
                        Console.WriteLine("Document group template updated successfully: {0}", response.Status ?? "No Content");
                    }
                    else
                    {
                        Console.WriteLine("Document group template updated successfully (204 No Content)");
                    }
                }
                else
                {
                    Console.WriteLine("No existing document group templates found. Skipping update example.");
                }
            }
            else
            {
                // Response contains data - verify the structure
                Assert.IsNotNull(documentGroupTemplate.Id);
                Assert.IsTrue(documentGroupTemplate.Id.Length == 40);
                Console.WriteLine("Created document group template: {0} with status: {1}", documentGroupTemplate.Id, documentGroupTemplate.Status);

                // Create update request with template order
                var updateRequest = new UpdateDocumentGroupTemplateRequest
                {
                    Order = new List<string> 
                    { 
                        template1.Id,
                        template2.Id
                    },
                    TemplateGroupName = "Updated Contract Template Group",
                    EmailActionOnComplete = EmailActionsType.DocumentsAndAttachments
                };

                // Update the document group template using the real template ID
                var response = await testContext.DocumentGroup
                    .UpdateDocumentGroupTemplateAsync(documentGroupTemplate.Id, updateRequest)
                    .ConfigureAwait(false);

                // Verify the response structure before accessing properties
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Status);
                Assert.AreEqual("success", response.Status);
                
                Console.WriteLine("Document group template updated successfully: {0}", response.Status);
            }

            // Clean up resources
            DeleteTestDocument(template1.Id);
            DeleteTestDocument(template2.Id);
            await testContext.DocumentGroup.DeleteDocumentGroupAsync(documentGroup.Id).ConfigureAwait(false);
            foreach (var document in documents)
            {
                DeleteTestDocument(document.Id);
            }
        }

        [TestMethod]
        public async Task CreateDocumentGroupTemplateAsync()
        {
            // This example demonstrates how to create a document group template from an existing document group.
            // Document group templates allow you to reuse document groups with predefined routing and signing workflows.
            
            // First, create a document group to use as source
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            
            var documents = new List<SignNowDocument>();
            for (int i = 0; i < 2; i++)
            {
                var upload = await testContext.Documents
                    .UploadDocumentAsync(fileStream, $"ForDocumentGroupTemplateFile-{i}.pdf");
                var doc = await testContext.Documents.GetDocumentAsync(upload.Id).ConfigureAwait(false);
                documents.Add(doc);
            }

            // Create document group from uploaded documents
            var documentGroup = await testContext.DocumentGroup
                .CreateDocumentGroupAsync("CreateDocumentGroupTemplateTest", documents)
                .ConfigureAwait(false);

            // Verify the document group was created
            Assert.IsTrue(documentGroup.Id.Length == 40);
            Console.WriteLine("Created document group: {0} with name {1}", documentGroup.Id, "CreateDocumentGroupTemplateTest");

            // Create document group template from the document group
            var createRequest = new CreateDocumentGroupTemplateRequest
            {
                Name = "Contract Template Group",
                OwnAsMerged = true
            };

            // Create the document group template
            // Note: This endpoint returns 202 Accepted with empty body for asynchronous processing
            var response = await testContext.DocumentGroup
                .CreateDocumentGroupTemplateAsync(documentGroup.Id, createRequest)
                .ConfigureAwait(false);

            // Verify the response structure before accessing properties
            Assert.IsNotNull(response);
            
            if (response.IsAccepted)
            {
                // 202 Accepted response with empty body - operation was scheduled
                Console.WriteLine("Document group template creation was accepted and scheduled for processing");
            }
            else
            {
                // Response contains data - verify the structure
                Assert.IsNotNull(response.Id);
                Assert.IsNotNull(response.Status);
                Assert.IsTrue(response.Id.Length == 40);
                Console.WriteLine("Document group template created successfully: {0} with status: {1}", response.Id, response.Status);
            }

            // Clean up resources
            await testContext.DocumentGroup.DeleteDocumentGroupAsync(documentGroup.Id).ConfigureAwait(false);
            foreach (var document in documents)
            {
                DeleteTestDocument(document.Id);
            }
        }

        /// <summary>
        /// This example demonstrates how to get a list of document group templates owned by the user.
        /// </summary>
        [TestMethod]
        public async Task GetDocumentGroupTemplatesAsync()
        {
            // Create request with pagination parameters
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 10,  // Get up to 10 templates
                Offset = 0   // Start from the first template
            };

            // Get document group templates
            var response = await testContext.DocumentGroup
                .GetDocumentGroupTemplatesAsync(request)
                .ConfigureAwait(false);

            // Verify the response structure before accessing properties
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DocumentGroupTemplates);
            Assert.IsTrue(response.DocumentGroupTemplateTotalCount >= 0);

            Console.WriteLine("Found {0} document group templates (total: {1})", 
                response.DocumentGroupTemplates.Count, 
                response.DocumentGroupTemplateTotalCount);

            // Display information about each template
            foreach (var template in response.DocumentGroupTemplates)
            {
                Console.WriteLine("Template Group ID: {0}", template.TemplateGroupId);
                Console.WriteLine("Template Group Name: {0}", template.TemplateGroupName);
                Console.WriteLine("Owner Email: {0}", template.OwnerEmail);
                Console.WriteLine("Is Prepared: {0}", template.IsPrepared);
                Console.WriteLine("Number of Templates: {0}", template.Templates?.Count ?? 0);
                Console.WriteLine("Last Updated: {0}", template.LastUpdated);
                
                if (template.Templates != null && template.Templates.Count > 0)
                {
                    Console.WriteLine("Templates in this group:");
                    foreach (var templateItem in template.Templates)
                    {
                        Console.WriteLine("  - Template ID: {0}", templateItem.Id);
                        Console.WriteLine("    Name: {0}", templateItem.Name);
                        Console.WriteLine("    Roles: {0}", string.Join(", ", templateItem.Roles ?? new List<string>()));
                    }
                }
                
                if (template.RoutingDetails != null)
                {
                    Console.WriteLine("Routing Details:");
                    Console.WriteLine("  - Sign as Merged: {0}", template.RoutingDetails.SignAsMerged);
                    Console.WriteLine("  - Invite Steps: {0}", template.RoutingDetails.InviteSteps?.Count ?? 0);
                }
                
                Console.WriteLine("---");
            }
        }

        /// <summary>
        /// This example demonstrates how to get document group templates with different pagination settings.
        /// </summary>
        [TestMethod]
        public async Task GetDocumentGroupTemplatesWithPaginationAsync()
        {
            // Get first page with 5 templates
            var firstPageRequest = new GetDocumentGroupTemplatesRequest
            {
                Limit = 5,
                Offset = 0
            };

            var firstPageResponse = await testContext.DocumentGroup
                .GetDocumentGroupTemplatesAsync(firstPageRequest)
                .ConfigureAwait(false);

            Assert.IsNotNull(firstPageResponse);
            Console.WriteLine("First page: {0} templates", firstPageResponse.DocumentGroupTemplates.Count);

            // If there are more templates, get the second page
            if (firstPageResponse.DocumentGroupTemplateTotalCount > 5)
            {
                var secondPageRequest = new GetDocumentGroupTemplatesRequest
                {
                    Limit = 5,
                    Offset = 5
                };

                var secondPageResponse = await testContext.DocumentGroup
                    .GetDocumentGroupTemplatesAsync(secondPageRequest)
                    .ConfigureAwait(false);

                Assert.IsNotNull(secondPageResponse);
                Console.WriteLine("Second page: {0} templates", secondPageResponse.DocumentGroupTemplates.Count);

                // Verify that we got different templates on the second page
                if (firstPageResponse.DocumentGroupTemplates.Count > 0 && secondPageResponse.DocumentGroupTemplates.Count > 0)
                {
                    var firstPageIds = firstPageResponse.DocumentGroupTemplates.Select(t => t.TemplateGroupId).ToHashSet();
                    var secondPageIds = secondPageResponse.DocumentGroupTemplates.Select(t => t.TemplateGroupId).ToHashSet();
                    
                    // Should not have any overlapping IDs between pages
                    Assert.IsFalse(firstPageIds.Intersect(secondPageIds).Any(), 
                        "Pages should not contain overlapping template IDs");
                }
            }
        }

        /// <summary>
        /// This example demonstrates how to get a specific document group template by ID from the list.
        /// </summary>
        [TestMethod]
        public async Task GetSpecificDocumentGroupTemplateAsync()
        {
            // First, get all available templates
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 50, // Get more templates to increase chances of finding one
                Offset = 0
            };

            var response = await testContext.DocumentGroup
                .GetDocumentGroupTemplatesAsync(request)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DocumentGroupTemplates);

            if (response.DocumentGroupTemplates.Count > 0)
            {
                // Get the first template for demonstration
                var template = response.DocumentGroupTemplates.First();
                
                Console.WriteLine("Found template: {0} (ID: {1})", 
                    template.TemplateGroupName, 
                    template.TemplateGroupId);
                
                // Display detailed information about this specific template
                Console.WriteLine("Template Details:");
                Console.WriteLine("  - ID: {0}", template.TemplateGroupId);
                Console.WriteLine("  - Name: {0}", template.TemplateGroupName);
                Console.WriteLine("  - Owner: {0}", template.OwnerEmail);
                Console.WriteLine("  - Is Prepared: {0}", template.IsPrepared);
                Console.WriteLine("  - Last Updated: {0}", template.LastUpdated);
                Console.WriteLine("  - Folder ID: {0}", template.FolderId ?? "None");
                
                if (template.Templates != null && template.Templates.Count > 0)
                {
                    Console.WriteLine("  - Templates ({0}):", template.Templates.Count);
                    foreach (var templateItem in template.Templates)
                    {
                        Console.WriteLine("    * {0} (ID: {1})", templateItem.Name, templateItem.Id);
                        Console.WriteLine("      Roles: {0}", string.Join(", ", templateItem.Roles ?? new List<string>()));
                        
                        if (templateItem.Thumbnail != null)
                        {
                            Console.WriteLine("      Thumbnail URLs:");
                            Console.WriteLine("        Small: {0}", templateItem.Thumbnail.Small);
                            Console.WriteLine("        Medium: {0}", templateItem.Thumbnail.Medium);
                            Console.WriteLine("        Large: {0}", templateItem.Thumbnail.Large);
                        }
                    }
                }
                
                if (template.RoutingDetails != null)
                {
                    Console.WriteLine("  - Routing Details:");
                    Console.WriteLine("    Sign as Merged: {0}", template.RoutingDetails.SignAsMerged);
                    Console.WriteLine("    Include Email Attachments: {0}", template.RoutingDetails.IncludeEmailAttachments);
                    
                    if (template.RoutingDetails.InviteSteps != null && template.RoutingDetails.InviteSteps.Count > 0)
                    {
                        Console.WriteLine("    Invite Steps ({0}):", template.RoutingDetails.InviteSteps.Count);
                        foreach (var step in template.RoutingDetails.InviteSteps)
                        {
                            Console.WriteLine("      Step {0}:", step.Order);
                            Console.WriteLine("        Emails: {0}", step.InviteEmails?.Count ?? 0);
                            Console.WriteLine("        Actions: {0}", step.InviteActions?.Count ?? 0);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No document group templates found for this user.");
            }
        }
    }
}
