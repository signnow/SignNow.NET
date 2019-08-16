using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.SignNow;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void DocumentUploadWorksForPDF()
        {
            using (var fileStream = File.OpenRead(pdfFilePath))
            {
                string docId = default;
                try
                {
                    var uploadResponse = docService.UploadDocumentAsync(fileStream).Result;
                    docId = uploadResponse.Id;
                    Assert.IsNotNull(uploadResponse.Id, "Document Upload result should contain non-null Id property value on successful upload");
                }
                finally
                {
                    DeleteDocument(docId);
                }

            }
        }

        [TestMethod]
        public void DocumentUploadWithFieldExtractWorksForPDF()
        {
            using (var fileStream = File.OpenRead(pdfFilePath))
            {
                string docId = default;
                try
                {
                    var uploadResponse = docService.UploadDocumentWithFieldExtractAsync(fileStream).Result;
                    //TODO: test if fields were extracted correctly
                    docId = uploadResponse.Id;
                    Assert.IsNotNull(uploadResponse.Id, "Document Upload result should contain non-null Id property value on successful upload");
                }
                finally
                {
                    DeleteDocument(docId);
                }
                
            }
        }

        [TestMethod]
        public void DocumentUploadExceptionIsCorrect()
        {
            DocumentUploadException(docService.UploadDocumentAsync);
        }

        [TestMethod]
        public void DocumentUploadWithFieldExtractExceptionIsCorrect()
        {
            DocumentUploadException(docService.UploadDocumentWithFieldExtractAsync);
        }

        void DocumentUploadException (Func<Stream, CancellationToken, Task<UploadDocumentResponse>> uploadFunction)
        {
            using (var fileStream = File.OpenRead(txtFilePath))
            {
                string docId = default;
                try
                {
                    var uploadResponse = uploadFunction(fileStream,default).Result;
                    docId = uploadResponse.Id;

                }
                catch (SignNowException ex)
                {
                    Assert.AreEqual(ErrorMessages.InvalidFileType, ex.Message);
                }
                finally
                {
                    DeleteDocument(docId);
                }

            }
        }
        void DeleteDocument(string id)
        {
            //docService.DeleteDocumentAsync(id).RunSynchronously();
        }
    }
}
