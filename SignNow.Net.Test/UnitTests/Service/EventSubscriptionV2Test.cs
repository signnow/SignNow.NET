using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Service;
using UnitTests;

namespace UnitTests
{
    [TestClass]
    public class EventSubscriptionV2Test : SignNowTestBase
    {
        [TestMethod]
        public async Task CreateEventSubscriptionV2Async_Success()
        {
            var mockClient = SignNowClientMock("{}");
            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), mockClient);
            var request = new CreateEventSubscriptionV2(
                EventType.DocumentComplete,
                "a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4",
                new Uri("https://example.com/webhook"));

            await service.CreateEventSubscriptionV2Async(request, CancellationToken.None);
            // No exception = success (void return)
        }

        [TestMethod]
        public async Task CreateEventSubscriptionV2Async_NullCallback_Throws()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
            {
                var _ = new CreateEventSubscriptionV2(
                    EventType.DocumentComplete,
                    "a1b2c3d4e5f6a1b2c3d4e5f6a1b2c3d4a1b2c3d4",
                    null);
                return Task.CompletedTask;
            });
        }
    }
}
