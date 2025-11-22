using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples
{
    [TestClass]
    public class GetEventSubscriptionById : ExamplesBase
    {
        /// <summary>
        /// Gets subscription info by subscription ID using the v2 event-subscriptions endpoint.
        /// This example demonstrates how to create an event subscription and then retrieve its details using the subscription ID.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/manage-event-subscriptions/operations/get-a-v-2-event-subscription"/>
        [TestMethod]
        public async Task GetEventSubscriptionByIdAsync()
        {
            // Upload document with fields
            await using var fileStream = File.OpenRead(PdfWithoutFields);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentForEventSubscriptionById.pdf")
                .ConfigureAwait(false);

            // Create event subscription for document update event
            var callbackUrl = new Uri($"https://example.com/{Guid.NewGuid()}"); // generate uniquie id which we will use to find the event subscription
            await testContext.Events
                .CreateEventSubscriptionAsync(new CreateEventSubscription(EventType.DocumentUpdate, document.Id, callbackUrl))
                .ConfigureAwait(false);

            // Get the list of event subscriptions to find our created subscription
            var eventSubscriptions = await testContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions 
                {
                    CallbackUrlFilter = CallbackUrlFilter.Like(callbackUrl.ToString())
                })
                .ConfigureAwait(false);

            var createdSubscription = eventSubscriptions.Data.FirstOrDefault();
            Assert.IsNotNull(createdSubscription, "Should have at least one event subscription");

            var subscriptionId = createdSubscription.Id;

            // Use GetEventSubscriptionByIdAsync to get subscription details
            var retrievedSubscription = await testContext.Events
                .GetEventSubscriptionByIdAsync(subscriptionId)
                .ConfigureAwait(false);

            // Verify the subscription details
            Assert.IsNotNull(retrievedSubscription);
            Assert.AreEqual(subscriptionId, retrievedSubscription.Id);
            Assert.AreEqual(EventType.DocumentUpdate, retrievedSubscription.Event);
            Assert.AreEqual(document.Id, retrievedSubscription.EntityUid);
            Assert.AreEqual("post", retrievedSubscription.RequestMethod);
            Assert.AreEqual("callback", retrievedSubscription.Action);
            Assert.AreEqual(EventSubscriptionEntityType.Document, retrievedSubscription.EntityType);
            Assert.IsNotNull(retrievedSubscription.JsonAttributes);
            Assert.AreEqual(callbackUrl, retrievedSubscription.JsonAttributes.CallbackUrl);

            Console.WriteLine($"Subscription ID: {retrievedSubscription.Id}");
            Console.WriteLine($"Entity Type: {retrievedSubscription.EntityType}");
            Console.WriteLine($"Event Type: {retrievedSubscription.Event}");
            Console.WriteLine($"Callback URL: {retrievedSubscription.JsonAttributes.CallbackUrl}");
            Console.WriteLine($"Created: {retrievedSubscription.Created}");

            // todo: update to v2 DeleteEventSubscription
            // Clean up
            await testContext.Events
                .DeleteEventSubscriptionAsync(subscriptionId)
                .ConfigureAwait(false);

            DeleteTestDocument(document.Id);
        }
    }
}
