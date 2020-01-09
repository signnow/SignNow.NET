using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Test;
using SignNow.Net.Test.Constants;
using System;
using System.IO;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void SigningLinkCreatedSuccessfully()
        {
            foreach (var docServiceTest in DocServices)
            {
                using (var fileStream = File.OpenRead(PdfFilePath))
                {
                    var uploadResponse = docServiceTest.UploadDocumentWithFieldExtractAsync(fileStream, pdfFileName).Result;
                    DocumentId = uploadResponse.Id;
                    var signingLinks = docServiceTest.CreateSigningLinkAsync(DocumentId).Result;

                    Assert.IsNotNull(signingLinks.Url);
                    Assert.IsNotNull(signingLinks.AnonymousUrl);
                }
            }
        }

        [TestMethod]
        public void SigningLinkExceptionIsCorrect()
        {
            foreach (var docServiceTest in DocServices)
            {
                try
                {
                    var links = docServiceTest.CreateSigningLinkAsync("Some Wrong Document Id").Result;
                }
                catch (AggregateException ex)
                {
                    Assert.AreEqual(ErrorMessages.TheDocumentIdShouldHave40Characters, ex.InnerException.Message);
                }
            }
        }
    }
}
