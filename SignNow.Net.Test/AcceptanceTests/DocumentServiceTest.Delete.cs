using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SignNow.Net.Test.Constants;

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
                "Document Delete result has error, status code is not Successful");
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

            Assert.IsFalse(regex.IsMatch(documentId), documentId);

            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => Task.WaitAll(deleteResponse));

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, ErrorMessages.InvalidDocumentId, documentId),
                exception.InnerException?.Message);
        }
    }
}
