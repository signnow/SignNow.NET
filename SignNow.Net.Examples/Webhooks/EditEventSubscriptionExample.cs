using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.EventSubscriptionBase;

namespace SignNow.Net.Examples
{
    public partial class EventSubscriptionExamples : ExamplesBase
    {
        /// <summary>
        /// Edit an existing event subscription.
        /// This example shows how to update an existing event subscription's properties like event type, 
        /// callback URL, and additional configuration options.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/reference/operations/put-v2-event-subscriptions-subscription_id"/>
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

            // Find the created subscription
            var eventSubscriptionList = await testContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions 
                { 
                    CallbackUrlFilter = CallbackUrlFilter.Like(originalCallbackUrl.ToString())
                })
                .ConfigureAwait(false);

            var subscriptionToEdit = eventSubscriptionList.Data.FirstOrDefault();
            Assert.IsNotNull(subscriptionToEdit, "Created event subscription not found");

            // Edit the event subscription with new configuration
            var updatedCallbackUrl = new Uri("https://example.com/updated-webhook");
            var updateRequest = new UpdateEventSubscription(
                EventType.DocumentUpdate, 
                document.Id,
                subscriptionToEdit.Id,
                updatedCallbackUrl)
            {
                Attributes = new EventCreateAttributes
                {
                    UseTls12 = true,
                    IncludeMetadata = true,
                    DocIdQueryParam = true,
                    DeleteAccessToken = false,
                },
                SecretKey = "my-secret-key-for-hmac"
            };

            await testContext.Events
                .EditEventSubscriptionAsync(updateRequest)
                .ConfigureAwait(false);

            // Verify the changes
            var updatedSubscription = await testContext.Events
                .GetEventSubscriptionAsync(subscriptionToEdit.Id)
                .ConfigureAwait(false);

            // Check updated properties
            Assert.AreEqual(EventType.DocumentUpdate, updatedSubscription.Event);
            Assert.AreEqual(updatedCallbackUrl, updatedSubscription.JsonAttributes.CallbackUrl);
            Assert.AreEqual(true, updatedSubscription.JsonAttributes.UseTls12);
            Assert.AreEqual(true, updatedSubscription.JsonAttributes.DocIdQueryParam);

            Console.WriteLine($"Successfully edited event subscription: {updatedSubscription.Id}");
            Console.WriteLine($"Event type changed to: {updatedSubscription.Event}");
            Console.WriteLine($"Callback URL updated to: {updatedSubscription.JsonAttributes.CallbackUrl}");
            Console.WriteLine($"TLS 1.2 enabled: {updatedSubscription.JsonAttributes.UseTls12}");

            // Cleanup - delete the event subscription
            await testContext.Events
                .DeleteEventSubscriptionAsync(subscriptionToEdit.Id)
                .ConfigureAwait(false);

            // Cleanup - delete the document
            await testContext.Documents
                .DeleteDocumentAsync(document.Id)
                .ConfigureAwait(false);
        }
    }
}
