using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using SignNow.Net.Exceptions;

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
        public async Task CannotDeleteDocumentWithWrongId()
        {
            var documentId = "test";

            var exception = await Assert
                .ThrowsExceptionAsync<ArgumentException>(
                    async () => await SignNowTestContext.Documents.DeleteDocumentAsync(documentId).ConfigureAwait(false))
                .ConfigureAwait(false);

            StringAssert.Contains(exception.Message, string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfId, documentId));
        }
    }
}
