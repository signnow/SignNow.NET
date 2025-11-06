using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Services
{
    [TestClass]
    public class EventSubscriptionServiceTest : SignNowTestBase
    {
        [TestMethod]
        public async Task GetEventSubscriptionsListAsync_WithOptions()
        {
            var fakeResponse = new EventSubscriptionResponseFaker().Generate();
            var responseJson = TestUtils.SerializeToJsonFormatted(fakeResponse);

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(responseJson));
            var options = new GetEventSubscriptionsListOptions
            {
                Page = 1,
                SortByCreated = SortOrder.Descending,
                IncludeEventCount = true
            };

            var response = await service
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            Assert.AreEqual(fakeResponse.Data.Count, response.Data.Count);
            Assert.AreEqual(fakeResponse.Meta.Pagination.Total, response.Meta.Pagination.Total);
        }

        [TestMethod]
        public async Task GetEventSubscriptionsListAsync_WithoutOptions()
        {
            var fakeResponse = new EventSubscriptionResponseFaker().Generate();
            var responseJson = TestUtils.SerializeToJsonFormatted(fakeResponse);

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(responseJson));

            var response = await service
                .GetEventSubscriptionsListAsync()
                .ConfigureAwait(false);

            Assert.AreEqual(fakeResponse.Data.Count, response.Data.Count);
            Assert.AreEqual(fakeResponse.Meta.Pagination.Total, response.Meta.Pagination.Total);
        }

        [TestMethod]
        public async Task GetEventSubscriptionsListAsync_WithComplexFilters()
        {
            var fakeResponse = new EventSubscriptionResponseFaker().Generate();
            var responseJson = TestUtils.SerializeToJsonFormatted(fakeResponse);

            var service = new EventSubscriptionService(ApiBaseUrl, new Token(), SignNowClientMock(responseJson));
            var options = new GetEventSubscriptionsListOptions
            {
                Page = 2,
                SortByEvent = SortOrder.Ascending,
                EntityAndCallbackUrlFilter = "test_entity",
                DateFilter = new DateRangeFilter(1580719773, 1580720509),
                EventTypeFilter = new[] { EventType.DocumentComplete },
                ApplicationFilter = new[] { "test_app" },
                IncludeEventCount = false
            };

            var response = await service
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            Assert.AreEqual(fakeResponse.Data.Count, response.Data.Count);
            Assert.AreEqual(fakeResponse.Meta.Pagination.Total, response.Meta.Pagination.Total);
        }
    }
}