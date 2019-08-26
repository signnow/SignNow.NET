using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Interfaces;
using SignNow.Net.Test;
using SignNow.Net.Test.SignNow;
using System;
using System.Collections.Generic;
using System.IO;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        private IEnumerable<IDocumentService> DocServices => new IDocumentService[] {new SignNowContext(ApiBaseUrl, Token).Documents };
        [TestMethod]
        public void SigningLinkCreatedSuccessfully()
        {
            foreach (var docService in DocServices)
            {
                using (var fileStream = File.OpenRead(PdfFilePath))
                {
                    string docId = default;
                    try
                    {
                        var uploadResponse = docService.UploadDocumentWithFieldExtractAsync(fileStream, pdfFileName).Result;
                        docId = uploadResponse.Id;
                        var signingLinks = docService.CreateSigningLinkAsync(docId).Result;
                        Assert.IsNotNull(signingLinks.Url);
                        Assert.IsNotNull(signingLinks.AnonymousUrl);
                    }
                    finally
                    {
                        DeleteDocument(docId);
                    }

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
