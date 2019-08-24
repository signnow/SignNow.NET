using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test;
using System.IO;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void DocumentDeletingSuccess()
        {
            var testDocumentId = UploadTestDocument(PdfFilePath);

            var deleteResponse = docService.DeleteDocumentAsync(testDocumentId).Result;

            Assert.AreEqual(
                "success",
                deleteResponse.Status,
                "Document Delete result has error, status shoul contains 'success' for successful deletion"
                );
        }

        private string UploadTestDocument(string filePath)
        {
            string docId = default;

            using (var fileStream = File.OpenRead(filePath))
            {
                var uploadResponse = docService.UploadDocumentAsync(fileStream, pdfFileName).Result;
                docId = uploadResponse.Id;

                Assert.IsNotNull(
                    uploadResponse.Id,
                    "Document Upload result should contain non-null Id property value on successful upload"
                    );
            }

            return docId;
        }
    }
}
