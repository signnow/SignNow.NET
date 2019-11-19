using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void DownloadDocumentAsPDF()
        {
            DocumentId = UploadTestDocument(PdfFilePath);

            var downloadResponse = docService.DownloadDocumentAsync(DocumentId).Result;

            Assert.IsTrue(downloadResponse.Document.CanRead, "Not readable Document content");
            Assert.AreEqual(downloadResponse.Filename, "DocumentUpload.pdf", "Wrong Document name");
            Assert.IsNotNull(downloadResponse.Length, "Document is Empty or not exists");
        }
    }
}
