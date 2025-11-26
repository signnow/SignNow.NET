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
    public partial class EventSubscriptionServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task DeleteEventSubscriptionAsync_WithValidId_DeletesSuccessfully()
        {
            var callbackUrl = new Uri($"https://example.com/delete-test{Faker.Random.Guid()}");
            await SignNowTestContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentComplete, TestPdfDocumentId, callbackUrl)
            ).ConfigureAwait(false);

            var options = new GetEventSubscriptionsListOptions
            {
                CallbackUrlFilter = CallbackUrlFilter.Like(callbackUrl.ToString())
            };

            var eventSubscriptions = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            var subscriptionToDelete = eventSubscriptions.Data.FirstOrDefault();
            Assert.IsNotNull(subscriptionToDelete, "Test subscription event not found");

            await SignNowTestContext.Events
                .DeleteEventSubscriptionAsync(subscriptionToDelete.Id)
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
                    .DeleteEventSubscriptionAsync(nonExistentId)
                    .ConfigureAwait(false)
            );

            Assert.AreEqual(HttpStatusCode.NotFound, exception.HttpStatusCode, "Should receive a 404 error for non-existent subscription");
            StringAssert.Contains(exception.Message, "Event subscription not found");
        }
    }
}
