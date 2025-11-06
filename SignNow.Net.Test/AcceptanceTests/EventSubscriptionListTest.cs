using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class EventSubscriptionListTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task GetEventSubscriptionsListAsync_WithBasicOptions()
        {
            // Act - Get event subscriptions with basic options
            var options = new GetEventSubscriptionsListOptions
            {
                Page = 1,
                IncludeEventCount = true
            };

            var response = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.IsNotNull(response.Meta.Pagination);
            Assert.IsTrue(response.Meta.Pagination.Total >= 0);
        }

        [TestMethod]
        public async Task GetEventSubscriptionsListAsync_WithSorting()
        {
            // Act - Get event subscriptions with sorting
            var options = new GetEventSubscriptionsListOptions
            {
                SortByCreated = SortOrder.Descending,
                Page = 1
            };

            var response = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            
            // If there are multiple subscriptions, verify they are sorted by creation date descending
            if (response.Data.Count > 1)
            {
                var sortedData = response.Data.OrderByDescending(x => x.Created).ToList();
                for (int i = 0; i < response.Data.Count; i++)
                {
                    Assert.AreEqual(sortedData[i].Id, response.Data[i].Id, 
                        "Event subscriptions should be sorted by creation date in descending order");
                }
            }
        }

        [TestMethod]
        public async Task GetEventSubscriptionsListAsync_WithoutOptions()
        {
            // Act - Get event subscriptions without any options
            var response = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync()
                .ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.IsNotNull(response.Meta.Pagination);
            Assert.IsTrue(response.Meta.Pagination.Total >= 0);
        }

        [TestMethod]
        public async Task GetEventSubscriptionsListAsync_WithEventTypeFilter()
        {
            // Act - Get event subscriptions filtered by event type
            var options = new GetEventSubscriptionsListOptions
            {
                EventTypeFilter = new[] { EventType.DocumentComplete, EventType.DocumentUpdate },
                Page = 1
            };

            var response = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            
            // If there are results, verify they match the filter
            foreach (var subscription in response.Data)
            {
                Assert.IsTrue(
                    subscription.Event == EventType.DocumentComplete || 
                    subscription.Event == EventType.DocumentUpdate,
                    $"Event subscription should have event type DocumentComplete or DocumentUpdate, but was {subscription.Event}");
            }
        }

        [TestMethod]
        public async Task GetEventSubscriptionsListAsync_CompareWithOriginalMethod()
        {
            // Act - Get results from both methods
            var originalResponse = await SignNowTestContext.Events
                .GetEventSubscriptionsAsync(new PagePaginationOptions { Page = 1 })
                .ConfigureAwait(false);

            var newResponse = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions { Page = 1 })
                .ConfigureAwait(false);

            // Assert - Both should return data (structure might be similar for basic queries)
            Assert.IsNotNull(originalResponse);
            Assert.IsNotNull(newResponse);
            Assert.IsNotNull(originalResponse.Data);
            Assert.IsNotNull(newResponse.Data);
            Assert.IsNotNull(originalResponse.Meta);
            Assert.IsNotNull(newResponse.Meta);

            // Both methods should handle pagination
            Assert.IsNotNull(originalResponse.Meta.Pagination);
            Assert.IsNotNull(newResponse.Meta.Pagination);
        }
    }
}