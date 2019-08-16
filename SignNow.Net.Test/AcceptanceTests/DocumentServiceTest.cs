using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Service;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    [TestClass]
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        readonly IDocumentService docService;
        readonly string pdfFilePath = "..\\..\\..\\TestData\\Documents\\DocumentUpload.pdf";
        readonly string txtFilePath = "..\\..\\..\\TestData\\Documents\\DocumentUpload.jpg";
        public DocumentServiceTest()
        {
            docService = new DocumentService(ApiBaseUrl, Token);
        }
    }
}
