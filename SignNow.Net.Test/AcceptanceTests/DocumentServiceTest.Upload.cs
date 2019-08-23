using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.SignNow;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void DocumentUploadWorksForPDF()
        {
            using (var fileStream = File.OpenRead(PdfFilePath))
            {
                string docId = default;
                try
                {
                    var uploadResponse = docService.UploadDocumentAsync(fileStream, pdfFileName).Result;
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
            using (var fileStream = File.OpenRead(PdfFilePath))
            {
                string docId = default;
                try
                {
                    var uploadResponse = docService.UploadDocumentWithFieldExtractAsync(fileStream, pdfFileName).Result;
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

        void DocumentUploadException (Func<Stream, string, CancellationToken, Task<UploadDocumentResponse>> uploadFunction)
        {
            using (var fileStream = File.OpenRead(TxtFilePath))
            {
                string docId = default;
                try
                {
                    var uploadResponse = uploadFunction(fileStream, txtFileName,default).Result;
                    docId = uploadResponse.Id;

                }
                catch (AggregateException ex)
                {
                    
                    Assert.AreEqual(ErrorMessages.InvalidFileType, ex.InnerException.Message);
                }
                finally
                {
                    DeleteDocument(docId);
                }

            }
        }
        void DeleteDocument(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;
            using (var client = new HttpClient())
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"{ApiBaseUrl}/document/{id}"))
            {
                requestMessage.Headers.Add("Authorization", Token.GetAccessToken());
                var response = client.SendAsync(requestMessage).Result;
                response.EnsureSuccessStatusCode();
            }
                
            //docService.DeleteDocumentAsync(id).RunSynchronously();
        }
    }
}
