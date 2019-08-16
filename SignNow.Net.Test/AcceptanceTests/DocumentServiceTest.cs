using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Service;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    [TestClass]
    public class DocumentServiceTest : AuthorizedApiTestBase
    {
        readonly IDocumentService docService;
        public DocumentServiceTest()
        {
            docService = new DocumentService(Token);
        }
        [TestMethod]
        public void DocumentUploadWithFieldExtract()
        {
        }
    }
}
