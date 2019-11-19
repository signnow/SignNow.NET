using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test;
using SignNow.Net.Test.SignNow;
using System;
using System.IO;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void SigningLinkCreatedSuccessfully()
        {
            foreach (var docService in DocServices)
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
        }

        [TestMethod]
        public void SigningLinkExceptionIsCorrect()
        {
            foreach (var docService in DocServices)
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
}
