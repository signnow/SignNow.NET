using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using SignNow.Net.Internal.Extensions;
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

        [TestMethod]
        public void CannotDeleteDocumentWithWrongId()
        {
            var documentId = "test";
            var deleteResponse = docService.DeleteDocumentAsync(documentId);

            Assert.ThrowsException<ArgumentException>(
                documentId.ValidateId);

            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => Task.WaitAll(deleteResponse));

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, ErrorMessages.InvalidFormatOfId, documentId),
                exception.InnerException?.Message);
        }
    }
}
