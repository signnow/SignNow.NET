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
    public class EditEventSubscriptionExample : ExamplesBase
    {
        /// <summary>
        /// Edit an existing event subscription.
        /// This example shows how to update an existing event subscription's properties like event type, 
        /// callback URL, and additional configuration options.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/manage-event-subscriptions/operations/update-a-v-2-event-subscription"/>
        [TestMethod]
        public async Task EditEventSubscriptionAsync()
        {
            // Upload document with fields
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentForEventSubscriptionEdit.pdf")
                .ConfigureAwait(false);

            // Create initial event subscription
            var originalCallbackUrl = new Uri("https://example.com/original-webhook");
            await testContext.Events
                .CreateEventSubscriptionAsync(new CreateEventSubscription(EventType.DocumentComplete, document.Id, originalCallbackUrl))
                .ConfigureAwait(false);

            // Find the created subscription by entity id (for EventType.DocumentComplete it is document id)
            var eventSubscriptionList = await testContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions 
                {
                    EntityIdFilter = EntityIdFilter.Like(document.Id)
                })
                .ConfigureAwait(false);

            var subscriptionToEdit = eventSubscriptionList.Data.FirstOrDefault();
            Assert.IsNotNull(subscriptionToEdit, "Created event subscription not found");

            // Edit the event subscription with new configuration
            var updatedCallbackUrl = new Uri("https://example.com/updated-webhook");
            var editRequest = new EditEventSubscription(EventType.DocumentUpdate, document.Id, subscriptionToEdit.Id, updatedCallbackUrl)
            {
                Attributes =
                {
                    UseTls12 = true,
                    IncludeMetadata = true
                }
            };

            await testContext.Events
                .EditEventSubscriptionAsync(editRequest)
                .ConfigureAwait(false);

            // Verify the changes
            var updatedSubscription = await testContext.Events
                .GetEventSubscriptionAsync(subscriptionToEdit.Id)
                .ConfigureAwait(false);

            Assert.AreEqual(EventType.DocumentUpdate, updatedSubscription.Event);
            Assert.AreEqual(updatedCallbackUrl, updatedSubscription.JsonAttributes.CallbackUrl);
            Assert.IsTrue(updatedSubscription.JsonAttributes.UseTls12);
            Assert.IsTrue(updatedSubscription.JsonAttributes.IncludeMetadata);

            Console.WriteLine($"Successfully edited event subscription: {updatedSubscription.Id}");
            Console.WriteLine($"Event type changed to: {updatedSubscription.Event}");
            Console.WriteLine($"Callback URL updated to: {updatedSubscription.JsonAttributes.CallbackUrl}");
            Console.WriteLine($"TLS 1.2 enabled: {updatedSubscription.JsonAttributes.UseTls12}");
            Console.WriteLine($"Include metadata updated to: {updatedSubscription.JsonAttributes.IncludeMetadata}");

            // Cleanup - delete the event subscription and document
            await testContext.Events
                .DeleteEventSubscriptionAsync(subscriptionToEdit.Id)
                .ConfigureAwait(false);

            await testContext.Documents
                .DeleteDocumentAsync(document.Id)
                .ConfigureAwait(false);
        }
    }
}
