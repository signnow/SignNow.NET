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
    public partial class GetEventSubscriptionById : ExamplesBase
    {
        /// <summary>
        /// Gets subscription info by subscription ID using the v2 event-subscriptions endpoint.
        /// This example demonstrates how to create an event subscription and then retrieve its details using the subscription ID.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/branches/v1.2/reference/operations/get-v2-event-subscriptions-subscription_id"/>
        [TestMethod]
        public async Task GetEventSubscriptionByIdAsync()
        {
            // Upload document with fields
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentForEventSubscriptionById.pdf")
                .ConfigureAwait(false);

            // Create event subscription for document update event
            var callbackUrl = new Uri("https://example.com/webhook/handler");
            await testContext.Events
                .CreateEventSubscriptionAsync(new CreateEventSubscription(EventType.DocumentUpdate, document.Id, callbackUrl))
                .ConfigureAwait(false);

            // Get the list of event subscriptions to find our created subscription
            var eventSubscriptions = await testContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions 
                { 
                    EntityIdFilter = EntityIdFilter.Like(document.Id),
                    EventTypeFilter = EventTypeFilter.In(EventType.DocumentUpdate),
                    PerPage = 1 
                })
                .ConfigureAwait(false);

            Assert.IsTrue(eventSubscriptions.Data.Count > 0, "Should have at least one event subscription");

            var createdSubscription = eventSubscriptions.Data.First();
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
            Assert.IsNotNull(retrievedSubscription.JsonAttributes);
            Assert.AreEqual(callbackUrl, retrievedSubscription.JsonAttributes.CallbackUrl);
            Assert.IsTrue(retrievedSubscription.Created > DateTime.MinValue);

            // The retrieved subscription should have additional properties from the v2 API response
            // such as entity_type that may not be available in other endpoints
            Console.WriteLine($"Subscription ID: {retrievedSubscription.Id}");
            Console.WriteLine($"Entity Type: {retrievedSubscription.EntityType}");
            Console.WriteLine($"Event Type: {retrievedSubscription.Event}");
            Console.WriteLine($"Callback URL: {retrievedSubscription.JsonAttributes.CallbackUrl}");
            Console.WriteLine($"Created: {retrievedSubscription.Created}");

            // Clean up: Delete the event subscription and document
            await testContext.Events
                .DeleteEventSubscriptionAsync(subscriptionId)
                .ConfigureAwait(false);

            DeleteTestDocument(document.Id);
        }
    }
}