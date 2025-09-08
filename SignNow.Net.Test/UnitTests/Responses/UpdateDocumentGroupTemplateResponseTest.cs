using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Responses;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Responses
{
    [TestClass]
    public class UpdateDocumentGroupTemplateResponseTest
    {
        [TestMethod]
        public void UpdateDocumentGroupTemplateResponseSerializationTest()
        {
            var response = new UpdateDocumentGroupTemplateResponseFaker().Generate();
            
            Assert.IsNotNull(response.Status);
            Assert.AreEqual("success", response.Status);
            
            // Verify that the response can be serialized to JSON
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Assert.IsFalse(string.IsNullOrEmpty(json));
            
            // Verify JSON contains expected properties
            Assert.IsTrue(json.Contains("status"));
            Assert.IsTrue(json.Contains("success"));
        }

        [TestMethod]
        public void UpdateDocumentGroupTemplateResponseDeserializationTest()
        {
            var json = @"{
                ""status"": ""success""
            }";
            
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateDocumentGroupTemplateResponse>(json);
            
            Assert.IsNotNull(response);
            Assert.AreEqual("success", response.Status);
        }

        [TestMethod]
        public void UpdateDocumentGroupTemplateResponseWithDifferentStatusTest()
        {
            var response = new UpdateDocumentGroupTemplateResponse
            {
                Status = "error"
            };

            Assert.AreEqual("error", response.Status);
            
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            Assert.IsTrue(json.Contains("error"));
        }
    }
}
