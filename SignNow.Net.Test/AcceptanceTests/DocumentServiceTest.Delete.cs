using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
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

        [DataRow("da057e65e97c9fa6b96485ca4970260f30958001", "internal api error")]
        [DataRow("da057e65e97c9fa6b", @"Unable to find a route to match the URI: document/da057e65e97c9fa6b")]
        [DataRow("test", @"Unable to find a route to match the URI: document/test")]
        [DataTestMethod]
        public void Cannot_Delete_Document_with_wrong_ID(string documentId, string errorMessage)
        {
            var deleteResponse = docService.DeleteDocumentAsync(documentId);

            try
            {
                Task.WaitAll(deleteResponse);
            }
            catch (SignNowException ex)
            {
                Assert.AreEqual(ex.Message, errorMessage);
            }
            catch
            {
                Assert.AreEqual(deleteResponse.Exception.InnerException.Message, errorMessage);
            }
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
