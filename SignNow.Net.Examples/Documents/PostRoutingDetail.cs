using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task PostRoutingDetailAsync()
        {
            // Note: This example demonstrates how to create or update routing details for a document
            // The API will create routing details based on actors data if they don't exist
            // If routing details already exist but are not active, it will update them
            
            // First, upload a document to test with
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "PostRoutingDetailTest.pdf")
                .ConfigureAwait(false);

            try
            {
                // Create or update routing detail information for the document
                var routingDetail = await testContext.Documents
                    .PostRoutingDetailAsync(document.Id)
                    .ConfigureAwait(false);

                // Display routing details information
                System.Console.WriteLine($"Invite Link Instructions: {routingDetail.InviteLinkInstructions}");
                
                // Display routing details (signers)
                foreach (var detail in routingDetail.RoutingDetails)
                {
                    System.Console.WriteLine($"Signer: {detail.Name}");
                    System.Console.WriteLine($"  Email: {detail.DefaultEmail}");
                    System.Console.WriteLine($"  Role ID: {detail.RoleId}");
                    System.Console.WriteLine($"  Signer Order: {detail.SignerOrder}");
                    System.Console.WriteLine($"  Inviter Role: {detail.InviterRole}");
                }

                // Display CC recipients
                if (routingDetail.Cc?.Count > 0)
                {
                    System.Console.WriteLine("CC Recipients:");
                    foreach (var ccEmail in routingDetail.Cc)
                    {
                        System.Console.WriteLine($"  {ccEmail}");
                    }
                }

                // Display CC steps
                if (routingDetail.CcStep?.Count > 0)
                {
                    System.Console.WriteLine("CC Steps:");
                    foreach (var ccStep in routingDetail.CcStep)
                    {
                        System.Console.WriteLine($"  Step {ccStep.Step}: {ccStep.Name} ({ccStep.Email})");
                    }
                }

                // Verify the response structure
                Assert.IsNotNull(routingDetail);
                Assert.IsNotNull(routingDetail.RoutingDetails);
                Assert.IsNotNull(routingDetail.Cc);
                Assert.IsNotNull(routingDetail.CcStep);
                Assert.IsNotNull(routingDetail.InviteLinkInstructions);
            }
            catch (SignNowException ex)
            {
                // If the document doesn't have actors or routing details can't be created, the API might return an error
                System.Console.WriteLine($"Could not create routing details: {ex.Message}");
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