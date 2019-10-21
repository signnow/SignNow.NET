using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Test;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

        [DataRow("da057e65e97c9fa6b96485ca4970260f309580019999")]
        [DataRow("da057e65e97c9fa6b9648")]
        [DataRow("test")]
        [DataRow("#")]
        [DataRow("--test-------------------------------------")]
        [DataRow("_valid_part_of_document_id_____________!")]
        [DataRow("000000000000000000000000000000000000000 ")]
        [DataTestMethod]
        public void Cannot_Delete_Document_With_Wrong_ID(string documentId)
        {
            var deleteResponse = docService.DeleteDocumentAsync(documentId);

            var regex = new Regex(@"^[a-zA-Z0-9_]{40,40}$");
            var message = "Invalid format of Document Id <" + documentId + ">. The required format: 40 characters long, case-sensitive, letters and numbers, underscore allowed.";

            Assert.IsFalse(regex.IsMatch(documentId), documentId);

            try
            {
                Task.WaitAll(deleteResponse);
            }
            catch (AggregateException ex)
            {
                foreach (var exception in ex.InnerExceptions)
                {
                    Console.WriteLine(documentId);
                    Assert.AreEqual(message, exception.Message);

                }
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
