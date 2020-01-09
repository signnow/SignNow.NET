using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Service;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    [TestClass]
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        private readonly IDocumentService docService;

        public string DocumentId { get; set; }

        [TestCleanup]
        public void TestCleanup()
        {
            DeleteDocument(DocumentId);
        }

        public DocumentServiceTest()
        {
            docService = new DocumentService(Token);
        }
    }
}
