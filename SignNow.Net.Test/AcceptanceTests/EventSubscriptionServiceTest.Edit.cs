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
        public async Task EditEventSubscriptionAsync_WithValidOptions_EditsSuccessfully()
        {
            var originalCallbackUrl = new Uri($"https://example.com/edit-test-original{Faker.Random.Guid()}");
            var updatedCallbackUrl = new Uri($"https://example.com/edit-test-updated{Faker.Random.Guid()}");

            // Create initial event subscription
            await SignNowTestContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentComplete, TestPdfDocumentId, originalCallbackUrl)
            ).ConfigureAwait(false);

            // Find the created subscription
            var options = new GetEventSubscriptionsListOptions
            {
                CallbackUrlFilter = CallbackUrlFilter.Like(originalCallbackUrl.ToString())
            };

            var eventSubscriptions = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            var subscriptionToEdit = eventSubscriptions.Data.FirstOrDefault();
            Assert.IsNotNull(subscriptionToEdit, "Test subscription event not found");

            // Edit the event subscription
            var updateRequest = new UpdateEventSubscription(
                EventType.DocumentUpdate, 
                TestPdfDocumentId,
                subscriptionToEdit.Id,
                updatedCallbackUrl)
            {
                Attributes = new SignNow.Net.Model.Requests.EventSubscriptionBase.EventCreateAttributes
                {
                    UseTls12 = true,
                    IncludeMetadata = true,
                    DocIdQueryParam = true
                }
            };

            await SignNowTestContext.Events
                .EditEventSubscriptionAsync(updateRequest)
                .ConfigureAwait(false);

            // Verify the changes
            var updatedSubscription = await SignNowTestContext.Events
                .GetEventSubscriptionAsync(subscriptionToEdit.Id)
                .ConfigureAwait(false);

            Assert.AreEqual(EventType.DocumentUpdate, updatedSubscription.Event, "Event type should be updated");
            Assert.AreEqual(updatedCallbackUrl, updatedSubscription.JsonAttributes.CallbackUrl, "Callback URL should be updated");
            Assert.AreEqual(true, updatedSubscription.JsonAttributes.UseTls12, "UseTls12 should be updated");
            Assert.AreEqual(true, updatedSubscription.JsonAttributes.DocIdQueryParam, "DocIdQueryParam should be updated");

            // Cleanup
            await SignNowTestContext.Events
                .DeleteEventSubscriptionAsync(subscriptionToEdit.Id)
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task EditEventSubscriptionAsync_WithNonExistentId_ThrowsSignNowException()
        {
            var nonExistentId = "1234567890abcdef1234567890abcdef12345678";
            var updateRequest = new UpdateEventSubscription(
                EventType.DocumentComplete, 
                TestPdfDocumentId,
                nonExistentId,
                new Uri("https://example.com/webhook"));

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await SignNowTestContext.Events
                    .EditEventSubscriptionAsync(updateRequest)
                    .ConfigureAwait(false)
            );

            Assert.AreEqual(HttpStatusCode.NotFound, exception.HttpStatusCode, "Should receive a 404 error for non-existent subscription");
            StringAssert.Contains(exception.Message, "Event subscription not found");
        }
    }
}
