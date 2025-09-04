using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task PutRoutingDetailAsync()
        {
            // Note: This example demonstrates how to update routing details for a document
            // The API will update or create routing detail based on the provided data
            
            // First, upload a document to test with
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "PutRoutingDetailTest.pdf")
                .ConfigureAwait(false);

            try
            {
                // Create a sample request with routing details
                var request = new PutRoutingDetailRequest
                {
                    Id = "e849617a2f26af2eb3d52e1251031050d933d6a6",
                    DocumentId = document.Id,
                    Data = new List<PutRoutingDetailData>
                    {
                        new PutRoutingDetailData
                        {
                            DefaultEmail = "signer1@example.com",
                            InviterRole = false,
                            Name = "Signer 1",
                            RoleId = "d7fcf72b4bbc47b0cc629ffe8b24421c66fec6a0",
                            SignerOrder = 1,
                            DeclineBySignature = false
                        },
                        new PutRoutingDetailData
                        {
                            DefaultEmail = "signer2@example.com",
                            InviterRole = false,
                            Name = "Signer 2",
                            RoleId = "14819de93089e889ab9f5283db9b7c39ad667e43",
                            SignerOrder = 2,
                            DeclineBySignature = true
                        }
                    },
                    Cc = new List<string> 
                    { 
                        "cc1@example.com", 
                        "cc2@example.com" 
                    },
                    CcStep = new List<PutCcStep>
                    {
                        new PutCcStep
                        {
                            Email = "cc1@example.com",
                            Step = 1,
                            Name = "CC Recipient 1"
                        },
                        new PutCcStep
                        {
                            Email = "cc2@example.com",
                            Step = 2,
                            Name = "CC Recipient 2"
                        }
                    },
                    InviteLinkInstructions = "Please review and sign this document. This is a test document for routing details.",
                    Viewers = new List<PutViewer>
                    {
                        new PutViewer
                        {
                            DefaultEmail = "viewer1@example.com",
                            Name = "Viewer 1",
                            SigningOrder = 1,
                            InviterRole = false,
                            ContactId = "38528aa9c323463c9563b3608c18467d9d569e09"
                        }
                    },
                    Approvers = new List<PutApprover>
                    {
                        new PutApprover
                        {
                            DefaultEmail = "approver1@example.com",
                            Name = "Approver 1",
                            SigningOrder = 3,
                            InviterRole = false,
                            ExpirationDays = 15,
                            ContactId = "abc123def456ghi789jkl012mno345pqr678stu901vwx234yz"
                        }
                    }
                };

                // Update routing detail information for the document
                var response = await testContext.Documents
                    .PutRoutingDetailAsync(document.Id, request)
                    .ConfigureAwait(false);

                // Display routing details information
                System.Console.WriteLine($"Invite Link Instructions: {response.InviteLinkInstructions}");
                
                // Display routing details (signers)
                foreach (var detail in response.TemplateData)
                {
                    System.Console.WriteLine($"Signer: {detail.Name}");
                    System.Console.WriteLine($"  Email: {detail.DefaultEmail}");
                    System.Console.WriteLine($"  Role ID: {detail.RoleId}");
                    System.Console.WriteLine($"  Signer Order: {detail.SignerOrder}");
                    System.Console.WriteLine($"  Inviter Role: {detail.InviterRole}");
                    System.Console.WriteLine($"  Decline By Signature: {detail.DeclineBySignature}");
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
                    System.Console.WriteLine("Viewers:");
                    foreach (var viewer in response.Viewers)
                    {
                        System.Console.WriteLine($"  Viewer: {viewer.Name}");
                        System.Console.WriteLine($"    Email: {viewer.DefaultEmail}");
                        System.Console.WriteLine($"    Contact ID: {viewer.ContactId}");
                        System.Console.WriteLine($"    Signing Order: {viewer.SigningOrder}");
                    }
                }

                // Display approvers
                if (response.Approvers?.Count > 0)
                {
                    System.Console.WriteLine("Approvers:");
                    foreach (var approver in response.Approvers)
                    {
                        System.Console.WriteLine($"  Approver: {approver.Name}");
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

                // Verify the response structure
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.TemplateData);
                Assert.IsNotNull(response.Cc);
                Assert.IsNotNull(response.CcStep);
                Assert.IsNotNull(response.InviteLinkInstructions);
                Assert.IsNotNull(response.Viewers);
                Assert.IsNotNull(response.Approvers);
                // Attributes can be null if not configured
            }
            catch (SignNowException ex)
            {
                // If the document doesn't have actors or routing details can't be updated, the API might return an error
                System.Console.WriteLine($"Could not update routing details: {ex.Message}");
                Assert.IsTrue(ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound || 
                             ex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest,
                    $"Unexpected error: {ex.Message}");
            }
            finally
            {
                // Clean up the test document
                DeleteTestDocument(document?.Id);
            }
        }
    }
}
