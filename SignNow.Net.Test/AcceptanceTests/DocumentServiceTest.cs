using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
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
    }
}
