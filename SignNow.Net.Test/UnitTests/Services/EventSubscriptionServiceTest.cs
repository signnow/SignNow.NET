using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using SignNow.Net.Service;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Services
{
    // todo: delete ?
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
    }
}
