using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test.Constants;
using System.IO;
using System.Threading.Tasks;
using SignNow.Net.Exceptions;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        private readonly string assertMsg =
            "Document Upload result should contain non-null Id property value on successful upload";

        [TestMethod]
        public async Task DocumentUploadWorksForPdf()
        {
            using var fileStream = File.OpenRead(PdfFilePath);
            var uploadResponse = await SignNowTestContext.Documents
                .UploadDocumentAsync(fileStream, PdfFileName)
                .ConfigureAwait(false);

            DisposableDocumentId = uploadResponse.Id;

            Assert.IsNotNull(uploadResponse.Id, assertMsg);
        }

        [TestMethod]
        public async Task DocumentUploadWithFieldExtractWorksForPdf()
        {
            using var fileStream = File.OpenRead(PdfFilePath);
            var uploadResponse = await SignNowTestContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, PdfFileName)
                .ConfigureAwait(false);

            DisposableDocumentId = uploadResponse.Id;

            Assert.IsNotNull(uploadResponse.Id, assertMsg);

            var document = await SignNowTestContext.Documents
                .GetDocumentAsync(DisposableDocumentId)
                .ConfigureAwait(false);

            // Check if fields were extracted (One field with role should be in the document)
            Assert.IsNotNull(document.Roles.Count);
        }

        [TestMethod]
        public async Task DocumentUploadExceptionIsCorrect()
        {
            using var fileStream = File.OpenRead(TxtFilePath);
            var exception = await Assert
                .ThrowsExceptionAsync<SignNowException>(
                    async () => await SignNowTestContext.Documents
                        .UploadDocumentAsync(fileStream, TxtFileName)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            Assert.AreEqual(ErrorMessages.InvalidFileType, exception.Message);
        }

        [TestMethod]
        public async Task DocumentUploadWithFieldExtractExceptionIsCorrect()
        {
            using var fileStream = File.OpenRead(TxtFilePath);
            var exception = await Assert
                .ThrowsExceptionAsync<SignNowException>(
                    async () => await SignNowTestContext.Documents
                        .UploadDocumentWithFieldExtractAsync(fileStream, TxtFileName)
                        .ConfigureAwait(false))
                .ConfigureAwait(false);

            Assert.AreEqual(ErrorMessages.InvalidFileType, exception.Message);
        }
    }
}
