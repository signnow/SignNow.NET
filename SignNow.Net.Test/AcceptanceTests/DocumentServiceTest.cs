using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Extensions;
using SignNow.Net.Model;
using SignNow.Net.Test;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void ShouldGetDocumentInfo()
        {
            var response = SignNowTestContext.Documents.GetDocumentAsync(TestPdfDocumentId).Result;

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
    }
}
