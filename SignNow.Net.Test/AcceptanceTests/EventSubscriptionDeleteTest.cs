using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class EventSubscriptionDeleteTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task DeleteEventSubscriptionAsync_WithValidId_DeletesSuccessfully()
        {
            await SignNowTestContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentComplete, TestPdfDocumentId, new Uri("https://example.com/delete-test"))
            ).ConfigureAwait(false);

            var options = new GetEventSubscriptionsListOptions
            {
                EntityIdFilter = EntityIdFilter.Like(TestPdfDocumentId)
            };

            var eventSubscriptions = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            var subscriptionToDelete = eventSubscriptions.Data.FirstOrDefault();
            Assert.IsNotNull(subscriptionToDelete, "Test subscription event not found");

            await SignNowTestContext.Events
                .DeleteEventSubscriptionV2Async(subscriptionToDelete.Id)
                .ConfigureAwait(false);

            var eventSubscriptionsAfterDelete = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            Assert.AreEqual(0, eventSubscriptionsAfterDelete.Data.Count, "Event subscription should have been deleted");
        }

        [TestMethod]
        public async Task DeleteEventSubscriptionAsync_WithNonExistentId_ThrowsSignNowException()
        {
            var nonExistentId = "1234567890abcdef1234567890abcdef12345678";

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await SignNowTestContext.Events
                    .DeleteEventSubscriptionV2Async(nonExistentId)
                    .ConfigureAwait(false)
            );

            Assert.AreEqual(HttpStatusCode.NotFound, exception.HttpStatusCode, "Should receive a 404 error for non-existent subscription");
            StringAssert.Contains(exception.Message, "Event subscription not found");
        }
    }
}
