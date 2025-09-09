using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Responses;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Responses
{
    [TestClass]
    public class CreateDocumentGroupTemplateResponseTest
    {
        [TestMethod]
        public void CreateDocumentGroupTemplateResponseSerializationTest()
        {
            var response = new CreateDocumentGroupTemplateResponseFaker().Generate();
            
            Assert.IsNotNull(response.Id);
            Assert.IsNotNull(response.Status);
            
            // Verify that the response can be serialized to JSON
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Assert.IsFalse(string.IsNullOrEmpty(json));
            
            // Verify JSON contains expected properties
            Assert.IsTrue(json.Contains("id"));
            Assert.IsTrue(json.Contains("status"));
        }

        [TestMethod]
        public void CreateDocumentGroupTemplateResponseWithDataTest()
        {
            var response = new CreateDocumentGroupTemplateResponse
            {
                Id = "b12e4a885b513a6d9c4c2e7c2b7fa06a013a7412",
                Status = "scheduled"
            };

            Assert.AreEqual("b12e4a885b513a6d9c4c2e7c2b7fa06a013a7412", response.Id);
            Assert.AreEqual("scheduled", response.Status);
        }

        [TestMethod]
        public void CreateDocumentGroupTemplateResponseWithSuccessStatusTest()
        {
            var response = new CreateDocumentGroupTemplateResponse
            {
                Id = "template123",
                Status = "success"
            };

            Assert.AreEqual("template123", response.Id);
            Assert.AreEqual("success", response.Status);
        }

        [TestMethod]
        public void CreateDocumentGroupTemplateResponseWithProcessingStatusTest()
        {
            var response = new CreateDocumentGroupTemplateResponse
            {
                Id = "template456",
                Status = "processing"
            };

            Assert.AreEqual("template456", response.Id);
            Assert.AreEqual("processing", response.Status);
        }
    }
}
