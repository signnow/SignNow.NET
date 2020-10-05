using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Extensions;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [TestMethod]
        public void DocumentDeletingSuccess()
        {
            DisposableDocumentId = UploadTestDocument(PdfFilePath);

            var deleteResponse = SignNowTestContext.Documents.DeleteDocumentAsync(DisposableDocumentId);
            Task.WaitAll(deleteResponse);

            Assert.IsFalse(
                deleteResponse.IsFaulted,
                "Document Delete result has error, status code is not Successful");
        }

        [TestMethod]
        public void CannotDeleteDocumentWithWrongId()
        {
            var documentId = "test";
            var deleteResponse = SignNowTestContext.Documents.DeleteDocumentAsync(documentId);

            Assert.ThrowsException<ArgumentException>(
                documentId.ValidateId);

            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => Task.WaitAll(deleteResponse));

            Assert.AreEqual(
                string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfId, documentId),
                exception.InnerException?.Message);
        }
    }
}
