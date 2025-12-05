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
        public async Task UpdateEventSubscriptionAsync_WithValidOptions_EditsSuccessfully()
        {
            var originalCallbackUrl = new Uri($"https://example.com/original-url/{Faker.Random.Guid()}"); // Guid is added, so we will be able to find only this event using CallbackUrlFilter
            var updatedCallbackUrl = new Uri("https://example.com/updated-url");

            await SignNowTestContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentComplete, TestPdfDocumentId, originalCallbackUrl)
            ).ConfigureAwait(false);

            var eventSubscriptions = await SignNowTestContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions {
                    CallbackUrlFilter = CallbackUrlFilter.Like(originalCallbackUrl.ToString())
                })
                .ConfigureAwait(false);

            var subscriptionToUpdate = eventSubscriptions.Data.FirstOrDefault();

            Assert.IsNotNull(subscriptionToUpdate, "Test subscription event not found");

            var updateRequest = new UpdateEventSubscription(EventType.DocumentUpdate, TestPdfDocumentId, subscriptionToUpdate.Id, updatedCallbackUrl)
            {
                Attributes =
                {
                    UseTls12 = true,
                    IncludeMetadata = true,
                    DocIdQueryParam = true
                }
            };

            var updateResponse = await SignNowTestContext.Events
                .UpdateEventSubscriptionAsync(updateRequest)
                .ConfigureAwait(false);

            var updatedSubscription = await SignNowTestContext.Events
                .GetEventSubscriptionAsync(updateResponse.Id)
                .ConfigureAwait(false);

            Assert.AreEqual(EventType.DocumentUpdate, updatedSubscription.Event, "Event type should be updated");
            Assert.AreEqual(updatedCallbackUrl, updatedSubscription.JsonAttributes.CallbackUrl, "Callback URL should be updated");
            Assert.IsTrue(updatedSubscription.JsonAttributes.UseTls12, "UseTls12 should be updated to true");
            Assert.IsTrue(updatedSubscription.JsonAttributes.IncludeMetadata, "UseTls12 should be updated to true");
            Assert.IsTrue(updatedSubscription.JsonAttributes.DocIdQueryParam, "DocIdQueryParam should be updated to true");

            await SignNowTestContext.Events
                .DeleteEventSubscriptionAsync(subscriptionToUpdate.Id)
                .ConfigureAwait(false);
        }

        [TestMethod]
        public async Task UpdateEventSubscriptionAsync_WithNonExistentId_ThrowsSignNowException()
        {
            var nonExistentId = "1234567890abcdef1234567890abcdef12345678";
            var updateRequest = new UpdateEventSubscription(
                EventType.DocumentComplete, 
                TestPdfDocumentId,
                nonExistentId,
                new Uri("https://example.com/webhook"));

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await SignNowTestContext.Events
                    .UpdateEventSubscriptionAsync(updateRequest)
                    .ConfigureAwait(false)
            );

            Assert.AreEqual(HttpStatusCode.NotFound, exception.HttpStatusCode, "Should receive a 404 error for non-existent subscription");
            StringAssert.Contains(exception.Message, "Event subscription not found");
        }
    }
}
