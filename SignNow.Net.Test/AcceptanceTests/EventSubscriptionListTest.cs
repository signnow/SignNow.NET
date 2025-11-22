using System;
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
        public async Task GetEventSubscriptionsListAsync_WithFilters()
        {
            await SignNowTestContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentFreeformSigned, TestPdfDocumentId, new Uri("https://docs.signnow.com"))
            ).ConfigureAwait(false);

            var options = new GetEventSubscriptionsListOptions
            {
                Page = 1,
                PerPage = 5,
                EventTypeFilter = EventTypeFilter.In(EventType.DocumentFreeformSigned),
                CallbackUrlFilter = CallbackUrlFilter.Like("docs.signnow"),
                EntityIdFilter = EntityIdFilter.Like(TestPdfDocumentId),
                SortByCreated = SortOrder.Descending
            };
            var response = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            Assert.AreEqual(
                $"filters=[{{\"entity_id\":{{\"type\": \"like\", \"value\":\"{TestPdfDocumentId}\"}}}}, " +
                $"{{\"callback_url\":{{\"type\": \"like\", \"value\":\"docs.signnow\"}}}}, " +
                $"{{\"event\":{{\"type\": \"in\", \"value\":[\"document.freeform.signed\"]}}}}]" +
                $"&sort[created]=desc&page=1&per_page=5",
                options.ToQueryString()
            );
            Assert.AreEqual(5, response.Meta.Pagination.PerPage);
            Assert.IsTrue(response.Data.Count > 0);
            var subscription = response.Data.First();
            Assert.AreEqual(EventType.DocumentFreeformSigned, subscription.Event);
            Assert.AreEqual(TestPdfDocumentId, subscription.EntityUid);
            Assert.AreEqual("post", subscription.RequestMethod);
            Assert.AreEqual(true, subscription.Active);

            // todo: update to v2 DeleteEventSubscription
            await SignNowTestContext.Events
                .DeleteEventSubscriptionAsync(response.Data.First().Id)
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GetEventSubscriptionByIdAsync_ReturnsSubscriptionDetails()
        {
            // Create a test event subscription
            await SignNowTestContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentUpdate, TestPdfDocumentId, new Uri("https://docs.signnow.com"))
            ).ConfigureAwait(false);

            // Get the list to find the created subscription
            var listResponse = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions
                {
                    EntityIdFilter = EntityIdFilter.Like(TestPdfDocumentId),
                    EventTypeFilter = EventTypeFilter.In(EventType.DocumentUpdate)
                })
                .ConfigureAwait(false);

            Assert.IsTrue(listResponse.Data.Count > 0, "Should have at least one subscription");
            
            var createdSubscription = listResponse.Data.First();
            var subscriptionId = createdSubscription.Id;

            // Test the new GetEventSubscriptionByIdAsync method
            var retrievedSubscription = await SignNowTestContext.Events
                .GetEventSubscriptionByIdAsync(subscriptionId)
                .ConfigureAwait(false);

            // Verify the retrieved subscription details
            Assert.IsNotNull(retrievedSubscription);
            Assert.AreEqual(subscriptionId, retrievedSubscription.Id);
            Assert.AreEqual(EventType.DocumentUpdate, retrievedSubscription.Event);
            Assert.AreEqual(TestPdfDocumentId, retrievedSubscription.EntityUid);
            Assert.AreEqual("post", retrievedSubscription.RequestMethod);
            Assert.AreEqual("callback", retrievedSubscription.Action);
            Assert.IsNotNull(retrievedSubscription.JsonAttributes);
            Assert.IsNotNull(retrievedSubscription.JsonAttributes.CallbackUrl);
            Assert.IsTrue(retrievedSubscription.Created > DateTime.MinValue);

            // Clean up: Delete the subscription
            await SignNowTestContext.Events
                .DeleteEventSubscriptionAsync(subscriptionId)
                .ConfigureAwait(false);
        }
    }
}
