using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace UnitTests.Requests
{
    [TestClass]
    public class CreateDocumentGroupTemplateRequestTest
    {

        [TestMethod]
        public void CreateDocumentGroupTemplateRequestWithRequiredFieldsTest()
        {
            var request = new CreateDocumentGroupTemplateRequest
            {
                Name = "Test Template Group"
            };

            Assert.AreEqual("Test Template Group", request.Name);
            Assert.IsNull(request.FolderId);
            Assert.IsNull(request.OwnAsMerged);
        }

        [TestMethod]
        public void CreateDocumentGroupTemplateRequestWithAllFieldsTest()
        {
            var request = new CreateDocumentGroupTemplateRequest
            {
                Name = "My Template Group",
                FolderId = "ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00777",
                OwnAsMerged = true
            };

            Assert.AreEqual("My Template Group", request.Name);
            Assert.AreEqual("ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00777", request.FolderId);
            Assert.AreEqual(true, request.OwnAsMerged);
        }

        [TestMethod]
        public void CreateDocumentGroupTemplateRequestWithOptionalFieldsTest()
        {
            var request = new CreateDocumentGroupTemplateRequest
            {
                Name = "Template Group",
                FolderId = "folder123",
                OwnAsMerged = false
            };

            Assert.AreEqual("Template Group", request.Name);
            Assert.AreEqual("folder123", request.FolderId);
            Assert.AreEqual(false, request.OwnAsMerged);
        }
    }
}
