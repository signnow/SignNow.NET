using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SignNow.Net.Service;
using SignNow.Net.Test;

namespace AcceptanceTests.Service
{
    [TestClass]
    public class DocumentServiceTest : ApiTestBase
    {
        [TestMethod]
        public void itCanDeleteDocument()
        {
            var documentService = new DocumentService(new SignNow.Net.Model.Token());

            var testTask = documentService.DeleteDocumentAsync("fakeDocumentId");

            Assert.IsFalse(testTask.IsFaulted);
        }
    }
}
