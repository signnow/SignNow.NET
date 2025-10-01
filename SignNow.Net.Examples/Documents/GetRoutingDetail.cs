using System.IO; // Added for File.OpenRead
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions; // Added for SignNowException
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task GetRoutingDetailAsync()
        {
            // Note: This example demonstrates how to get routing details for a document
            // In a real scenario, you would need to set up routing details first using the POST or PUT endpoints

            // First, upload a document to test with
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "GetRoutingDetailTest.pdf")
                .ConfigureAwait(false);

            // Get routing detail information for the document
            var routingDetail = await testContext.Documents
                .GetRoutingDetailAsync(document.Id)
                .ConfigureAwait(false);

            // Verify response structure first with assertions
            Assert.IsNotNull(routingDetail);
            Assert.IsNotNull(routingDetail.RoutingDetails);
            Assert.IsNotNull(routingDetail.Cc);
            Assert.IsNotNull(routingDetail.CcStep);
            Assert.IsNotNull(routingDetail.InviteLinkInstructions);
            Assert.IsNotNull(routingDetail.Viewers);
            Assert.IsNotNull(routingDetail.Approvers);

            // Display routing details information
            System.Console.WriteLine($"Invite Link Instructions: {routingDetail.InviteLinkInstructions}");

            // Display routing details (signers)
            foreach (var detail in routingDetail.RoutingDetails)
            {
                System.Console.WriteLine($"Signer: {detail.Name}");
                System.Console.WriteLine($"  Email: {detail.DefaultEmail}");
                System.Console.WriteLine($"  Role ID: {detail.RoleId}");
                System.Console.WriteLine($"  Signing Order: {detail.SigningOrder}");
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

            // Display viewers
            if (routingDetail.Viewers?.Count > 0)
            {
                System.Console.WriteLine("Viewers:");
                foreach (var viewer in routingDetail.Viewers)
                {
                    System.Console.WriteLine($"  Viewer: {viewer.Name}");
                    System.Console.WriteLine($"    Email: {viewer.DefaultEmail}");
                    System.Console.WriteLine($"    Contact ID: {viewer.ContactId}");
                    System.Console.WriteLine($"    Signing Order: {viewer.SigningOrder}");
                }
            }

            // Display approvers
            if (routingDetail.Approvers?.Count > 0)
            {
                System.Console.WriteLine("Approvers:");
                foreach (var approver in routingDetail.Approvers)
                {
                    System.Console.WriteLine($"  Approver: {approver.Name}");
                    System.Console.WriteLine($"    Email: {approver.DefaultEmail}");
                    System.Console.WriteLine($"    Signing Order: {approver.SigningOrder}");
                    System.Console.WriteLine($"    Expiration Days: {approver.ExpirationDays}");
                    if (approver.Authentication != null)
                    {
                        System.Console.WriteLine($"    Authentication Type: {approver.Authentication.Type}");
                    }
                }
            }

            // Display routing attributes
            if (routingDetail.Attributes != null)
            {
                System.Console.WriteLine("Routing Attributes:");
                System.Console.WriteLine($"  Brand ID: {routingDetail.Attributes.BrandId}");
                System.Console.WriteLine($"  Redirect URI: {routingDetail.Attributes.RedirectUri}");
                System.Console.WriteLine($"  On Complete: {routingDetail.Attributes.OnComplete}");
            }

            // Note: Attributes can be null if not configured, which is expected behavior

            // Clean up the test document
            DeleteTestDocument(document?.Id);
        }
    }
}
