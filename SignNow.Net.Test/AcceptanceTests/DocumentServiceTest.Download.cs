using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.SignNow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void DownloadDocumentAsPDF()
        {
            var testDocumentId = UploadTestDocument(PdfFilePath);

            var downloadResponse = docService.DownloadDocumentAsync(testDocumentId).Result;

            Assert.IsTrue(
                downloadResponse.CanRead,
                "Download document result has an error, status code is not Successful"
                );

            DeleteDocument(testDocumentId);
        }
    }
}
