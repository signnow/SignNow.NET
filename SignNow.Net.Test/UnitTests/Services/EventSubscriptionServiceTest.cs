using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Services
{
    [TestClass]
    public class EventSubscriptionServiceTest : SignNowTestBase
    {
        [TestMethod]
        public async Task GetEventSubscriptionByIdAsync_Should_Return_EventSubscription()
        {
            // Arrange
            var mockResponse = @"
            {
                ""data"": {
                    ""id"": ""a7abc123def456ghi789jklmnop01234567890ab"",
                    ""entity_type"": ""document"",
                    ""event"": ""document.update"",
                    ""entity_id"": 123456,
                    ""entity_unique_id"": ""4abc123def456ghi789jklmnop012345qrstuv89"",
                    ""request_method"": ""post"",
                    ""action"": ""callback"",
                    ""json_attributes"": {
                        ""use_tls_12"": true,
                        ""docid_queryparam"": true,
                        ""integration_id"": ""b9abc123def456ghi789jklmnop012345qrstuv01"",
                        ""callback_url"": ""http://example.com/handle?hash=3abcdefg"",
                        ""headers"": {
                            ""string_head"": ""string_head"",
                            ""int_head"": 12,
                            ""bool_head"": false,
                            ""float_head"": 12.24
                        },
                        ""secret_key"": ""sample_secret_key"",
                        ""delete_access_token"": true
                    },
                    ""application_name"": ""Sample appliacation"",
                    ""created"": 1658396537
                }
            }";

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var subscriptionId = "a7abc123def456ghi789jklmnop01234567890ab";

            // Act
            var result = await service.GetEventSubscriptionByIdAsync(subscriptionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(subscriptionId, result.Id);
            Assert.AreEqual("document", result.EntityType);
            Assert.AreEqual(EventType.DocumentUpdate, result.Event);
            Assert.AreEqual(123456, result.EntityId);
            Assert.AreEqual("4abc123def456ghi789jklmnop012345qrstuv89", result.EntityUid);
            Assert.AreEqual("post", result.RequestMethod);
            Assert.AreEqual("callback", result.Action);
            Assert.AreEqual("Sample appliacation", result.ApplicationName);
            Assert.IsNotNull(result.JsonAttributes);
            Assert.AreEqual(true, result.JsonAttributes.UseTls12);
            Assert.AreEqual(true, result.JsonAttributes.DocIdQueryParam);
            Assert.AreEqual("b9abc123def456ghi789jklmnop012345qrstuv01", result.JsonAttributes.IntegrationId);
            Assert.AreEqual("http://example.com/handle?hash=3abcdefg", result.JsonAttributes.CallbackUrl.ToString());
            Assert.AreEqual("sample_secret_key", result.JsonAttributes.SecretKey);
            Assert.AreEqual(true, result.JsonAttributes.DeleteAccessToken);
        }
    }
}