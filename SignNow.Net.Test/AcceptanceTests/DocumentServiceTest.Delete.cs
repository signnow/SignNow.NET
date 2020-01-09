using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [TestMethod]
        public void DocumentDeletingSuccess()
        {
            DocumentId = UploadTestDocument(PdfFilePath, docService);

            var deleteResponse = docService.DeleteDocumentAsync(DocumentId);
            Task.WaitAll(deleteResponse);

            Assert.IsFalse(
                deleteResponse.IsFaulted,
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
        public void CannotDeleteDocumentWithWrongId(string documentId)
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
                    Assert.AreEqual(message, exception.Message);
                }
            }
        }
    }
}
