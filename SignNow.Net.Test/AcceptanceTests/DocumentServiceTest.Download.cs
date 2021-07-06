using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [DataTestMethod]
        [DataRow(DownloadType.PdfOriginal, ".pdf")]
        [DataRow(DownloadType.PdfCollapsed, ".pdf")]
        [DataRow(DownloadType.PdfWithHistory, ".pdf")]
        [DataRow(DownloadType.ZipCollapsed, ".zip")]
        public async Task DownloadDocumentAsSpecifiedType(DownloadType downloadType, string expectedType)
        {
            var downloadResponse = await SignNowTestContext.Documents
                .DownloadDocumentAsync(TestPdfDocumentId, downloadType)
                .ConfigureAwait(false);

            StringAssert.Contains(downloadResponse.Filename, "DocumentUpload", "Wrong Document name");
            StringAssert.Contains(downloadResponse.Filename, expectedType, "Wrong Document type");

            if (downloadType == DownloadType.ZipCollapsed)
            {
                Assert.That.StreamIsZip(downloadResponse.Document);
            }
            else
            {
                Assert.That.StreamIsPdf(downloadResponse.Document);
            }
        }
    }
}
