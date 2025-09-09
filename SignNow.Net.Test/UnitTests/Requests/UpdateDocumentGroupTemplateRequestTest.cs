using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Requests
{
    [TestClass]
    public class UpdateDocumentGroupTemplateRequestTest
    {
        [TestMethod]
        public void UpdateDocumentGroupTemplateRequestSerializationTest()
        {
            var request = new UpdateDocumentGroupTemplateRequestFaker().Generate();
            
            Assert.IsNotNull(request.Order);
            Assert.IsNotNull(request.TemplateGroupName);
            Assert.IsNotNull(request.EmailActionOnComplete);
            
            // Verify that the request can be serialized to JSON
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            Assert.IsFalse(string.IsNullOrEmpty(json));
            
            // Verify JSON contains expected properties
            Assert.IsTrue(json.Contains("order"));
            Assert.IsTrue(json.Contains("template_group_name"));
            Assert.IsTrue(json.Contains("email_action_on_complete"));
        }

        [TestMethod]
        public void UpdateDocumentGroupTemplateRequestWithEmptyOrderTest()
        {
            var request = new UpdateDocumentGroupTemplateRequest
            {
                Order = new List<string>(),
                TemplateGroupName = "Test Template Group",
                EmailActionOnComplete = "documents_and_attachments"
            };

            Assert.AreEqual(0, request.Order.Count);
            Assert.AreEqual("Test Template Group", request.TemplateGroupName);
            Assert.AreEqual("documents_and_attachments", request.EmailActionOnComplete);
        }

        [TestMethod]
        public void UpdateDocumentGroupTemplateRequestWithDataTest()
        {
            var order = new List<string> { "template1", "template2" };
            var templateGroupName = "My Template Group";
            var emailActionOnComplete = "documents_and_attachments_only_to_recipients";

            var request = new UpdateDocumentGroupTemplateRequest
            {
                Order = order,
                TemplateGroupName = templateGroupName,
                EmailActionOnComplete = emailActionOnComplete
            };

            Assert.AreEqual(2, request.Order.Count);
            Assert.AreEqual("template1", request.Order[0]);
            Assert.AreEqual("template2", request.Order[1]);
            
            Assert.AreEqual(templateGroupName, request.TemplateGroupName);
            Assert.AreEqual(emailActionOnComplete, request.EmailActionOnComplete);
        }
    }
}
