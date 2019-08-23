using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Service;
using SignNow.Net.Test;
using System.IO;

namespace AcceptanceTests
{
    [TestClass]
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        readonly IDocumentService docService;
        readonly string testDocumentsPath = "..\\..\\..\\TestData\\Documents";
        readonly string pdfFileName = "DocumentUpload.pdf";
        readonly string txtFileName = "DocumentUpload.txt";

        private string PdfFilePath => Path.Combine(testDocumentsPath, pdfFileName);
        private string TxtFilePath => Path.Combine(testDocumentsPath, txtFileName);
        public DocumentServiceTest()
        {
            docService = new DocumentService(ApiBaseUrl, Token);
        }
    }
}
