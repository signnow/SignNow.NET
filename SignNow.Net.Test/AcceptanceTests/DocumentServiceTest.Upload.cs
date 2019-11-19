using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.SignNow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        delegate Task<UploadDocumentResponse> DocumentUploadDelegate (Stream fileStream, string fileName, CancellationToken cancellation);
        private IEnumerable<IDocumentService> DocServices => new IDocumentService[] { new SignNowContext(Token).Documents, new SignNowContext(ApiBaseUrl, Token).Documents };

        [TestMethod]
        public void DocumentUploadWorksForPDF()
        {
            DocumentUploadTestServices(DocumentUploadTest, d => d.UploadDocumentAsync);
        }

        [TestMethod]
        public void DocumentUploadWithFieldExtractWorksForPDF()
        {
            DocumentUploadTestServices(DocumentUploadTest, d => d.UploadDocumentWithFieldExtractAsync);
            //TODO: test if fields were extracted correctly
        }

        [TestMethod]
        public void DocumentUploadExceptionIsCorrect()
        {
            DocumentUploadTestServices(DocumentUploadExceptionTest, d => d.UploadDocumentAsync);
        }

        [TestMethod]
        public void DocumentUploadWithFieldExtractExceptionIsCorrect()
        {
            DocumentUploadTestServices(DocumentUploadExceptionTest, d => d.UploadDocumentWithFieldExtractAsync);
        }

        void DocumentUploadTestServices (Action<DocumentUploadDelegate> testAction, Func<IDocumentService, DocumentUploadDelegate> uploadMethodFactory)
        {
            foreach (var ds in DocServices)
                testAction(uploadMethodFactory(ds));
        }

        void DocumentUploadTest (DocumentUploadDelegate uploadFunction)
        {
            using (var fileStream = File.OpenRead(PdfFilePath))
            {
                string docId = default;
                try
                {
                    var uploadResponse = uploadFunction(fileStream, pdfFileName, default).Result;
                    docId = uploadResponse.Id;

                    Assert.IsNotNull(uploadResponse.Id, "Document Upload result should contain non-null Id property value on successful upload");
                }
                finally
                {
                    DeleteDocument(docId);
                }
            }
        }

        void DocumentUploadExceptionTest (DocumentUploadDelegate uploadFunction)
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
    }
}
