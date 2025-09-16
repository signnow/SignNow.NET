using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Model.EditFields;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;
using UpdateRoutingDetailCcStepRequest = SignNow.Net.Model.Requests.UpdateRoutingDetailCcStep;
using UpdateRoutingDetailViewerRequest = SignNow.Net.Model.Requests.UpdateRoutingDetailViewer;
using UpdateRoutingDetailApproverRequest = SignNow.Net.Model.Requests.UpdateRoutingDetailApprover;
using UnitTests;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task UpdateRoutingDetailAsync()
        {
            // Note: This example demonstrates how to update routing details for a document
            // The API will update or create routing detail based on the provided data
            
            // First, upload a document to test with
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "UpdateRoutingDetailTest.pdf")
                .ConfigureAwait(false);

            // Add fields with roles to the document
            var fields = new List<IFieldEditable>
            {
                new TextField
                {
                    PageNumber = 0,
                    Name = "TextName",
                    Role = "Signer 1",
                    Height = 100,
                    Width = 200,
                    Label = "LabelName",
                    PrefilledText = "prefilled-text-example",
                    Required = true,
                    X = 10,
                    Y = 20
                },
            };

            // Edit the document to add fields
            var editResponse = await testContext.Documents
                .EditDocumentAsync(document.Id, fields)
                .ConfigureAwait(false);

            // Create routing details first
            var createResponse = await testContext.Documents
                .CreateRoutingDetailAsync(document.Id)
                .ConfigureAwait(false);

            // Get the role ID from the created routing details
            var signerRoleId = createResponse.RoutingDetails?.FirstOrDefault()?.RoleId;
            if (string.IsNullOrEmpty(signerRoleId))
            {
                throw new InvalidOperationException("Failed to get signer role ID from created routing details");
            }

            // Create a sample request with routing details using real IDs
            var request = new UpdateRoutingDetailRequest
            {
                Id = createResponse.RoutingDetails?.FirstOrDefault()?.RoleId ?? signerRoleId,
                DocumentId = document.Id,
                Data = new List<RoutingDetailData>
                {
                    new RoutingDetailData
                    {
                        DefaultEmail = "signer1@example.com",
                        InviterRole = false,
                        Name = "Signer",
                        RoleId = signerRoleId,
                        SignerOrder = 1,
                        DeclineBySignature = false
                    }
                },
                Cc = new List<string>
                {
                    "cc1@example.com",
                    "cc2@example.com"
                },
                CcStep = new List<UpdateRoutingDetailCcStepRequest>
                {
                    new UpdateRoutingDetailCcStepRequest
                    {
                        Email = "cc1@example.com",
                        Step = 1,
                        Name = "CC Recipient 1"
                    },
                    new UpdateRoutingDetailCcStepRequest
                    {
                        Email = "cc2@example.com",
                        Step = 2,
                        Name = "CC Recipient 2"
                    }
                },
                InviteLinkInstructions = "Please review and sign this document. This is a test document for routing details.",
                Viewers = new List<UpdateRoutingDetailViewerRequest>
                {
                    new UpdateRoutingDetailViewerRequest
                    {
                        DefaultEmail = "viewer1@example.com",
                        Name = "Test Viewer",
                        SigningOrder = 1,
                        InviterRole = false
                    }
                },
                Approvers = new List<UpdateRoutingDetailApproverRequest>
                {
                    new UpdateRoutingDetailApproverRequest
                    {
                        DefaultEmail = "approver1@example.com",
                        Name = "Test Approver",
                        SigningOrder = 2,
                        InviterRole = false,
                        ExpirationDays = 15
                    }
                }
            };

            // Update routing detail information for the document
            var response = await testContext.Documents
                .UpdateRoutingDetailAsync(document.Id, request)
                .ConfigureAwait(false);


            // Verify response structure first with assertions
            Assert.IsNotNull(response);
            // Note: TemplateData can be null in some API responses
            Assert.IsNotNull(response.Cc);
            Assert.IsNotNull(response.CcStep);
            Assert.IsNotNull(response.InviteLinkInstructions);
            Assert.IsNotNull(response.Viewers);
            Assert.IsNotNull(response.Approvers);

            // Display routing details information
            System.Console.WriteLine($"Invite Link Instructions: {response.InviteLinkInstructions}");

            // Display routing details (signers)
            if (response.TemplateData?.Count > 0)
            {
                foreach (var detail in response.TemplateData)
                {
                    System.Console.WriteLine($"Signer: {detail.Name}");
                    System.Console.WriteLine($"  Email: {detail.DefaultEmail}");
                    System.Console.WriteLine($"  Role ID: {detail.RoleId}");
                    System.Console.WriteLine($"  Signer Order: {detail.SignerOrder}");
                    System.Console.WriteLine($"  Inviter Role: {detail.InviterRole}");
                    System.Console.WriteLine($"  Decline By Signature: {detail.DeclineBySignature}");
                }
            }
            else
            {
                System.Console.WriteLine("No template data available in response");
            }

            // Display CC recipients
            if (response.Cc?.Count > 0)
            {
                System.Console.WriteLine("CC Recipients:");
                foreach (var ccEmail in response.Cc)
                {
                    System.Console.WriteLine($"  {ccEmail}");
                }
            }

            // Display CC steps
            if (response.CcStep?.Count > 0)
            {
                System.Console.WriteLine("CC Steps:");
                foreach (var ccStep in response.CcStep)
                {
                    System.Console.WriteLine($"  Step {ccStep.Step}: {ccStep.Name} ({ccStep.Email})");
                }
            }

            // Display viewers
            if (response.Viewers?.Count > 0)
            {
                System.Console.WriteLine("UpdateRoutingDetailViewers:");
                foreach (var viewer in response.Viewers)
                {
                    System.Console.WriteLine($"  UpdateRoutingDetailViewer: {viewer.Name}");
                    System.Console.WriteLine($"    Email: {viewer.DefaultEmail}");
                    System.Console.WriteLine($"    Contact ID: {viewer.ContactId}");
                    System.Console.WriteLine($"    Signing Order: {viewer.SigningOrder}");
                }
            }

            // Display approvers
            if (response.Approvers?.Count > 0)
            {
                System.Console.WriteLine("UpdateRoutingDetailApprovers:");
                foreach (var approver in response.Approvers)
                {
                    System.Console.WriteLine($"  UpdateRoutingDetailApprover: {approver.Name}");
                    System.Console.WriteLine($"    Email: {approver.DefaultEmail}");
                    System.Console.WriteLine($"    Signing Order: {approver.SigningOrder}");
                    System.Console.WriteLine($"    Contact ID: {approver.ContactId}");
                }
            }

            // Display routing attributes
            if (response.Attributes != null)
            {
                System.Console.WriteLine("Routing Attributes:");
                System.Console.WriteLine($"  Brand ID: {response.Attributes.BrandId}");
                System.Console.WriteLine($"  Redirect URI: {response.Attributes.RedirectUri}");
                System.Console.WriteLine($"  Close Redirect URI: {response.Attributes.CloseRedirectUri}");
            }

            // Response structure has been verified at the beginning
            // Note: Attributes can be null if not configured, which is expected behavior

            // Clean up the test document
            DeleteTestDocument(document?.Id);
        }
    }
}
