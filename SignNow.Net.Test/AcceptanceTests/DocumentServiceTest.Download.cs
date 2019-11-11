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

            Assert.IsTrue(
                downloadResponse.CanRead,
                "Download document result has an error, status code is not Successful"
                );
        }
    }
}
