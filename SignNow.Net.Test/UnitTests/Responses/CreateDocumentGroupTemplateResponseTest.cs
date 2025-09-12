using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Responses;
using SignNow.Net.Test.FakeModels;
using UnitTests;

namespace UnitTests.Responses
{
    [TestClass]
    public class CreateDocumentGroupTemplateResponseTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            // Arrange
            var response = new CreateDocumentGroupTemplateResponseFaker().Generate();
            
            // Act
            var json = TestUtils.SerializeToJsonFormatted(response);
            var deserializedResponse = TestUtils.DeserializeFromJson<CreateDocumentGroupTemplateResponse>(json);
            
            // Assert
            Assert.IsNotNull(deserializedResponse);
            Assert.AreEqual(response.Id, deserializedResponse.Id);
            Assert.AreEqual(response.Status, deserializedResponse.Status);
        }

        [TestMethod]
        public void ShouldSerializeDeserializeRoundtrip_ForScheduledStatus()
        {
            // Arrange
            var originalResponse = new CreateDocumentGroupTemplateResponse
            {
                Id = "test-template-123",
                Status = "scheduled"
            };

            // Act
            var json = TestUtils.SerializeToJsonFormatted(originalResponse);
            var deserializedResponse = TestUtils.DeserializeFromJson<CreateDocumentGroupTemplateResponse>(json);

            // Assert
            Assert.AreEqual(originalResponse.Id, deserializedResponse.Id);
            Assert.AreEqual(originalResponse.Status, deserializedResponse.Status);
            Assert.That.JsonEqual(TestUtils.SerializeToJsonFormatted(originalResponse), deserializedResponse);
        }

        [TestMethod]
        public void ShouldSerializeDeserializeRoundtrip_ForSuccessStatus()
        {
            // Arrange
            var originalResponse = new CreateDocumentGroupTemplateResponse
            {
                Id = "test-template-456",
                Status = "success"
            };

            // Act
            var json = TestUtils.SerializeToJsonFormatted(originalResponse);
            var deserializedResponse = TestUtils.DeserializeFromJson<CreateDocumentGroupTemplateResponse>(json);

            // Assert
            Assert.AreEqual(originalResponse.Id, deserializedResponse.Id);
            Assert.AreEqual(originalResponse.Status, deserializedResponse.Status);
            Assert.That.JsonEqual(TestUtils.SerializeToJsonFormatted(originalResponse), deserializedResponse);
        }

        [TestMethod]
        public void ShouldHandleAcceptedResponse_WithNullIdAndAcceptedStatus()
        {
            // Arrange
            var response = new CreateDocumentGroupTemplateResponse
            {
                Id = null,
                Status = "accepted"
            };

            // Act & Assert
            Assert.IsTrue(response.IsAccepted);
            Assert.IsNull(response.Id);
            Assert.AreEqual("accepted", response.Status);
        }
    }
}
