using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using SignNow.Net.Exceptions;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [TestMethod]
        public void SigningLinkCreatedSuccessfully()
        {
            using (var fileStream = File.OpenRead(PdfFilePath))
            {
                var uploadResponse = SignNowTestContext.Documents.UploadDocumentWithFieldExtractAsync(fileStream, pdfFileName).Result;
                DocumentId = uploadResponse.Id;
                var signingLinks = SignNowTestContext.Documents.CreateSigningLinkAsync(DocumentId).Result;

                Assert.IsNotNull(signingLinks.Url);
                Assert.IsNotNull(signingLinks.AnonymousUrl);
            }
        }

        [TestMethod]
        public void SigningLinkExceptionIsCorrect()
        {
            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => SignNowTestContext.Documents.CreateSigningLinkAsync("Some Wrong Document Id").Result);

            var expected = string
                .Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfId, "Some Wrong Document Id");

            Assert.AreEqual(expected, exception.InnerException?.Message);
        }
    }
}
