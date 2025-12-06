using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Service;

namespace UnitTests.Services
{
    [TestClass]
    public class EventSubscriptionServiceTest : SignNowTestBase
    {
        [TestMethod]
        public async Task GetCallbacksAsyncTest()
        {
            var mockResponse = TestUtils.SerializeToJsonFormatted(new
            {
                data = new[]
                {
                    new
                    {
                        id = "callback_123",
                        entity_id = "doc_456",
                        callback_url = "https://example.com/webhook",
                        initiator_id = "user_789",
                        start_time = 1609459200,
                        end_time = 1609459205,
                        code = 200,
                        @event = "document.create",
                        event_type = "document",
                        application = "app_abc123",
                        response_body = "OK",
                        error_message = (string)null
                    }
                },
                meta = new
                {
                    pagination = new
                    {
                        total = 1,
                        count = 1,
                        per_page = 50,
                        current_page = 1,
                        total_pages = 1
                    }
                }
            });

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));
            var options = new GetCallbacksOptions
            {
                //SortByStartTime = SortOrder.Descending,
                //CodeFilter = CodeRangeFilter.Success()
            };

            var response = await service.GetCallbacksAsync(options)
                .ConfigureAwait(false);

            Assert.AreEqual(1, response.Data.Count);
            Assert.AreEqual("callback_123", response.Data[0].Id);
            Assert.AreEqual("doc_456", response.Data[0].EntityId);
            Assert.AreEqual("https://example.com/webhook", response.Data[0].CallbackUrl.ToString());
            //Assert.AreEqual("user_789", response.Data[0].InitiatorId);
            //Assert.AreEqual(200, response.Data[0].Code);
            //Assert.AreEqual("document.create", response.Data[0].Event);
            //Assert.AreEqual("document", response.Data[0].EventType);
            //Assert.AreEqual("app_abc123", response.Data[0].Application);
            //Assert.IsTrue(response.Data[0].IsSuccessful);
            //Assert.IsFalse(response.Data[0].IsClientError);
            //Assert.IsFalse(response.Data[0].IsServerError);
        }

        [TestMethod]
        public async Task GetCallbacksAsyncWithNoOptionsTest()
        {
            var mockResponse = TestUtils.SerializeToJsonFormatted(new
            {
                data = new object[0],
                meta = new
                {
                    pagination = new
                    {
                        total = 0,
                        count = 0,
                        per_page = 50,
                        current_page = 1,
                        total_pages = 0
                    }
                }
            });

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(mockResponse));

            var response = await service.GetCallbacksAsync()
                .ConfigureAwait(false);

            Assert.AreEqual(0, response.Data.Count);
            Assert.IsNotNull(response.Meta);
        }
    }
}
