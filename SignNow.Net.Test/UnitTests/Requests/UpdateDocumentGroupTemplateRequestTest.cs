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
            
            Assert.IsNotNull(request.TemplateIdsToAdd);
            Assert.IsNotNull(request.TemplateIdsToRemove);
            Assert.IsNotNull(request.RoutingDetails);
            Assert.IsNotNull(request.TemplateGroupName);
            
            // Verify that the request can be serialized to JSON
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            Assert.IsFalse(string.IsNullOrEmpty(json));
            
            // Verify JSON contains expected properties
            Assert.IsTrue(json.Contains("template_ids_to_add"));
            Assert.IsTrue(json.Contains("template_ids_to_remove"));
            Assert.IsTrue(json.Contains("routing_details"));
            Assert.IsTrue(json.Contains("template_group_name"));
        }

        [TestMethod]
        public void UpdateDocumentGroupTemplateRequestWithEmptyListsTest()
        {
            var request = new UpdateDocumentGroupTemplateRequest
            {
                TemplateIdsToAdd = new List<string>(),
                TemplateIdsToRemove = new List<string>(),
                RoutingDetails = "{}",
                TemplateGroupName = "Test Template Group"
            };

            Assert.AreEqual(0, request.TemplateIdsToAdd.Count);
            Assert.AreEqual(0, request.TemplateIdsToRemove.Count);
            Assert.AreEqual("{}", request.RoutingDetails);
            Assert.AreEqual("Test Template Group", request.TemplateGroupName);
        }

        [TestMethod]
        public void UpdateDocumentGroupTemplateRequestWithDataTest()
        {
            var templateIdsToAdd = new List<string> { "template1", "template2" };
            var templateIdsToRemove = new List<string> { "template3" };
            var routingDetails = "{\"invite_steps\":[]}";
            var templateGroupName = "My Template Group";

            var request = new UpdateDocumentGroupTemplateRequest
            {
                TemplateIdsToAdd = templateIdsToAdd,
                TemplateIdsToRemove = templateIdsToRemove,
                RoutingDetails = routingDetails,
                TemplateGroupName = templateGroupName
            };

            Assert.AreEqual(2, request.TemplateIdsToAdd.Count);
            Assert.AreEqual("template1", request.TemplateIdsToAdd[0]);
            Assert.AreEqual("template2", request.TemplateIdsToAdd[1]);
            
            Assert.AreEqual(1, request.TemplateIdsToRemove.Count);
            Assert.AreEqual("template3", request.TemplateIdsToRemove[0]);
            
            Assert.AreEqual(routingDetails, request.RoutingDetails);
            Assert.AreEqual(templateGroupName, request.TemplateGroupName);
        }
    }
}
