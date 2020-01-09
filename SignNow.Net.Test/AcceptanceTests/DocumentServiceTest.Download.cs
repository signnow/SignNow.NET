using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [DataTestMethod]
        [DataRow(DownloadType.PdfOriginal, ".pdf")]
        [DataRow(DownloadType.PdfCollapsed, ".pdf")]
        [DataRow(DownloadType.PdfWithHistory, ".pdf")]
        [DataRow(DownloadType.ZipCollapsed, ".zip")]
        public void DownloadDocumentAsSpecifiedType(DownloadType downloadType, string expectedType)
        {
            DocumentId = UploadTestDocument(PdfFilePath, docService);

            var downloadResponse = docService.DownloadDocumentAsync(DocumentId, downloadType).Result;

            StringAssert.Contains(downloadResponse.Filename, "DocumentUpload", "Wrong Document name");
            StringAssert.Contains(downloadResponse.Filename, expectedType, "Wrong Document type");

            Assert.IsTrue(downloadResponse.Document.CanRead, "Not readable Document content");
            Assert.IsNotNull(downloadResponse.Length, "Document is Empty or not exists");

            string actual;
            var expected = expectedType == ".pdf" ? "%PDF-1.3" : "PK";

            using (var reader = new StreamReader(downloadResponse.Document, Encoding.UTF8))
            {
                actual = reader.ReadLine();
            }

            StringAssert.StartsWith(actual, expected, "Document content is not a ZIP or PDF format");
        }
    }
}
