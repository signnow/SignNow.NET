using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test.Constants;
using System;
using System.IO;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [TestMethod]
        public void SigningLinkCreatedSuccessfully()
        {
            using (var fileStream = File.OpenRead(PdfFilePath))
            {
                var uploadResponse = docService.UploadDocumentWithFieldExtractAsync(fileStream, pdfFileName).Result;
                DocumentId = uploadResponse.Id;
                var signingLinks = docService.CreateSigningLinkAsync(DocumentId).Result;

                Assert.IsNotNull(signingLinks.Url);
                Assert.IsNotNull(signingLinks.AnonymousUrl);
            }
        }

        [TestMethod]
        public void SigningLinkExceptionIsCorrect()
        {
            try
            {
                var links = docService.CreateSigningLinkAsync("Some Wrong Document Id").Result;
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(ErrorMessages.TheDocumentIdShouldHave40Characters, ex.InnerException.Message);
            }
        }
    }
}
