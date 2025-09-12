using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace UnitTests.Requests
{
    [TestClass]
    public class UpdateDocumentGroupTemplateRequestTest
    {

        [TestMethod]
        public void UpdateDocumentGroupTemplateRequestWithEmptyOrderTest()
        {
            var request = new UpdateDocumentGroupTemplateRequest
            {
                Order = new List<string>(),
                TemplateGroupName = "Test Template Group",
                EmailActionOnComplete = EmailActionsType.DocumentsAndAttachments
            };

            Assert.AreEqual(0, request.Order.Count);
            Assert.AreEqual("Test Template Group", request.TemplateGroupName);
            Assert.AreEqual(EmailActionsType.DocumentsAndAttachments, request.EmailActionOnComplete);
        }

        [TestMethod]
        public void UpdateDocumentGroupTemplateRequestWithDataTest()
        {
            var order = new List<string> { "template1", "template2" };
            var templateGroupName = "My Template Group";
            var emailActionOnComplete = EmailActionsType.DocumentsAndAttachmentsOnlyToRecipients;

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
