using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class GetCallbacksBySubscriptionIdServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task GetCallbacksBySubscriptionIdAsync_WithFilters_ReturnsFilteredResults()
        {
            var eventSubscriptions = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync()
                .ConfigureAwait(false);

            if (eventSubscriptions?.Data == null || eventSubscriptions.Data.Count == 0)
            {
                Assert.Inconclusive("No event subscriptions available for testing");
                return;
            }

            var subscriptionId = eventSubscriptions.Data[0].Id;

            var options = new GetCallbacksOptions
            {
                Filters = f => f.Code.Between(200, 599),
                Sortings = s => s.StartTime(SortOrder.Descending),
                PerPage = 15
            };

            var response = await SignNowTestContext.Events
                .GetCallbacksBySubscriptionIdAsync(subscriptionId, options)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.AreEqual(15, response.Meta.Pagination.PerPage);

            // Verify all callbacks belong to the specified subscription and match filter criteria
            foreach (var callback in response.Data)
            {
                Assert.AreEqual(subscriptionId, callback.EventSubscriptionId);
                Assert.IsTrue(callback.ResponseStatusCode >= 200 && callback.ResponseStatusCode <= 299,
                    $"Callback status code {callback.ResponseStatusCode} should be in range 200-299");
            }
        }
    }
}
