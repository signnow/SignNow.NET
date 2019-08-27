using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test;
using System.IO;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void DocumentDeletingSuccess()
        {
            var testDocumentId = UploadTestDocument(PdfFilePath);

            var deleteResponse = docService.DeleteDocumentAsync(testDocumentId);
            Task.WaitAll(deleteResponse);

            Assert.IsTrue(
                deleteResponse.IsCompletedSuccessfully,
                "Document Delete result has error, status code is not Successful"
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
