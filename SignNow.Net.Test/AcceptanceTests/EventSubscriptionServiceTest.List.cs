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
    //[TestClass]
    public partial class EventSubscriptionServiceTest : AuthorizedApiTestBase
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
                .UnsubscribeEventSubscriptionAsync(response.Data.First().Id)
                .ConfigureAwait(false);
        }
    }
}
