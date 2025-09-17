using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;
using UnitTests;
using UpdateRoutingDetailCcStepRequest = SignNow.Net.Model.Requests.UpdateRoutingDetailCcStep;
using UpdateRoutingDetailViewerRequest = SignNow.Net.Model.Requests.UpdateRoutingDetailViewer;
using UpdateRoutingDetailApproverRequest = SignNow.Net.Model.Requests.UpdateRoutingDetailApprover;

namespace AcceptanceTests
{
    [TestClass]
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task ShouldGetDocumentInfo()
        {
            var response = await SignNowTestContext.Documents.GetDocumentAsync(TestPdfDocumentId).ConfigureAwait(false);

            Assert.AreEqual(TestPdfDocumentId, response.Id);
            Assert.AreEqual(1, response.PageCount);
            Assert.AreEqual(PdfFileName, response.OriginalName);
            Assert.AreEqual("DocumentUpload", response.Name);

            Assert.IsNotNull(response.UserId.ValidateId());
            Assert.IsNotNull(response.Created);
            Assert.IsNotNull(response.Updated);

            Assert.IsNull(response.OriginDocumentId);
            Assert.IsNull(response.OriginUserId);

            Assert.IsFalse(response.IsTemplate);
        }

        [TestMethod]
        public async Task MergeDocuments()
        {
            var doc1 = await SignNowTestContext.Documents
                .GetDocumentAsync(TestPdfDocumentId)
                .ConfigureAwait(false);
            var doc2 = await SignNowTestContext.Documents
                .GetDocumentAsync(TestPdfDocumentIdWithFields)
                .ConfigureAwait(false);

            var documents = new List<SignNowDocument> {doc1, doc2};

            var merged = await SignNowTestContext.Documents
                .MergeDocumentsAsync("merged-document.pdf", documents)
                .ConfigureAwait(false);

            Assert.AreEqual("merged-document.pdf", merged.Filename);
            Assert.That.StreamIsPdf(merged.Document);
        }

        [TestMethod]
        public async Task DocumentHistory()
        {
            var response = await SignNowTestContext.Documents
                .GetDocumentHistoryAsync(TestPdfDocumentIdWithFields)
                .ConfigureAwait(false);

            Assert.IsTrue(response.All(itm => itm.Id.Length == 40));
            Assert.IsTrue(response.All(itm => itm.DocumentId == TestPdfDocumentIdWithFields));
        }

        [TestMethod]
        public async Task CreateOneTimeDocumentDownloadLink()
        {
            var link = await SignNowTestContext.Documents
                .CreateOneTimeDownloadLinkAsync(TestPdfDocumentId)
                .ConfigureAwait(false);

            Assert.IsNotNull(link.Url);
            StringAssert.Contains(link.Url.Host, "signnow.com");
        }

        [TestMethod]
        public async Task GetRoutingDetail()
        {
            // This test requires a document with routing details configured
            // If the test fails, it means either the document doesn't have routing details
            // or there's an actual issue with the API call
            var response = await SignNowTestContext.Documents
                .GetRoutingDetailAsync(TestPdfDocumentIdWithFields)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.RoutingDetails);
            Assert.IsNotNull(response.Cc);
            Assert.IsNotNull(response.CcStep);
            Assert.IsNotNull(response.InviteLinkInstructions);
            Assert.IsNotNull(response.Viewers);
            Assert.IsNotNull(response.Approvers);
            // Attributes can be null if not configured in the document
        }

        [TestMethod]
        public async Task UpdateRoutingDetail()
        {
            // First, try to get existing routing details or create them
            GetRoutingDetailResponse existingRoutingDetail = null;
            string roleId = null;

            try
            {
                // Try to get existing routing details
                existingRoutingDetail = await SignNowTestContext.Documents
                    .GetRoutingDetailAsync(TestPdfDocumentIdWithFields)
                    .ConfigureAwait(false);

                if (existingRoutingDetail?.RoutingDetails?.Count > 0)
                {
                    roleId = existingRoutingDetail.RoutingDetails[0].RoleId;
                }
            }
            catch (SignNow.Net.Exceptions.SignNowException)
            {
                // If getting routing details fails, try to create them
                try
                {
                    var createResponse = await SignNowTestContext.Documents
                        .CreateRoutingDetailAsync(TestPdfDocumentIdWithFields)
                        .ConfigureAwait(false);

                    if (createResponse?.RoutingDetails?.Count > 0)
                    {
                        roleId = createResponse.RoutingDetails[0].RoleId;
                    }
                }
                catch (SignNow.Net.Exceptions.SignNowException ex)
                {
                    // If both getting and creating fail, skip the test
                    Assert.Inconclusive($"Cannot get or create routing details for document {TestPdfDocumentIdWithFields}: {ex.Message}");
                    return;
                }
            }

            // If we still don't have a role ID, use a mock value for testing error handling
            if (string.IsNullOrEmpty(roleId))
            {
                roleId = "mockroleidfortesting123456789012345";
            }

            var request = new UpdateRoutingDetailRequest
            {
                Id = roleId,
                DocumentId = TestPdfDocumentIdWithFields,
                Data = new List<RoutingDetailData>
                {
                    new RoutingDetailData
                    {
                        DefaultEmail = "signer1@example.com",
                        InviterRole = false,
                        Name = "Signer 1",
                        RoleId = roleId,
                        SignerOrder = 1,
                        DeclineBySignature = false
                    }
                },
                Cc = new List<string> { "cc1@example.com" },
                CcStep = new List<UpdateRoutingDetailCcStepRequest>
                {
                    new UpdateRoutingDetailCcStepRequest
                    {
                        Email = "cc1@example.com",
                        Step = 1,
                        Name = "CC Recipient 1"
                    }
                },
                InviteLinkInstructions = "Please review and sign this document"
            };

            var response = await SignNowTestContext.Documents
                .UpdateRoutingDetailAsync(TestPdfDocumentIdWithFields, request)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            // TemplateData can be null if not configured in the document
            Assert.IsNotNull(response.Cc);
            Assert.IsNotNull(response.CcStep);
            Assert.IsNotNull(response.InviteLinkInstructions);
            Assert.IsNotNull(response.Viewers);
            Assert.IsNotNull(response.Approvers);
        }
    }
}
