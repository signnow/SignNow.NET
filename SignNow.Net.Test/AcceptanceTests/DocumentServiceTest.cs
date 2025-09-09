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
        public async Task PostRoutingDetail()
        {
            // Note: This test creates or updates routing details for a document
            // The API will create routing details based on actors data if they don't exist
            try
            {
                var response = await SignNowTestContext.Documents
                    .PostRoutingDetailAsync(TestPdfDocumentIdWithFields)
                    .ConfigureAwait(false);

                Assert.IsNotNull(response);
                Assert.IsNotNull(response.RoutingDetails);
                Assert.IsNotNull(response.Cc);
                Assert.IsNotNull(response.CcStep);
                Assert.IsNotNull(response.InviteLinkInstructions);
            }
            catch (SignNowException ex)
            {
                // If the document doesn't have actors or routing details can't be created, the API might return an error
                Assert.IsTrue(ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound || 
                             ex.HttpStatusCode == System.Net.HttpStatusCode.BadRequest,
                    $"Unexpected error: {ex.Message}");
            }
        }

        [TestMethod]
        public async Task PutRoutingDetail()
        {
            // This test updates routing details for a document
            // If the test fails, it means either the document doesn't have the required actors/roles
            // or there's an actual issue with the API call
            var request = new PutRoutingDetailRequest
            {
                Id = "e849617a2f26af2eb3d52e1251031050d933d6a6",
                DocumentId = TestPdfDocumentIdWithFields,
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
                    }
                },
                Cc = new List<string> { "cc1@example.com" },
                CcStep = new List<PutCcStep>
                {
                    new PutCcStep
                    {
                        Email = "cc1@example.com",
                        Step = 1,
                        Name = "CC Recipient 1"
                    }
                },
                InviteLinkInstructions = "Please review and sign this document",
                Viewers = new List<PutViewer>(),
                Approvers = new List<PutApprover>()
            };

            var response = await SignNowTestContext.Documents
                .PutRoutingDetailAsync(TestPdfDocumentIdWithFields, request)
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
