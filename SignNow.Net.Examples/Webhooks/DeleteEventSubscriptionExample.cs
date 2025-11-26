using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;

namespace SignNow.Net.Examples
{
    public partial class EventSubscriptionExamples : ExamplesBase
    {
        /// <summary>
        /// Demonstrates how to delete an event subscription using the SignNow .NET SDK.
        /// This example shows the complete workflow: create, list, and delete an event subscription.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/manage-event-subscriptions/operations/delete-a-v-2-event-subscription"/>
        [TestMethod]
        public async Task DeleteEventSubscriptionExampleAsync()
        {
            // Upload a test document
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var document = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "DocumentForEventSubscriptionDelete.pdf")
                .ConfigureAwait(false);

            Console.WriteLine($"✓ Uploaded document with ID: {document.Id}");

            // Create an event subscription
            var callbackUrl = new Uri("https://example.com/webhook/document-complete");
            await testContext.Events
                .CreateEventSubscriptionAsync(new CreateEventSubscription(EventType.DocumentComplete, document.Id, callbackUrl))
                .ConfigureAwait(false);

            Console.WriteLine($"✓ Created event subscription for DocumentComplete events");

            // Create filter to find the event subscription we just created
            var options = new GetEventSubscriptionsListOptions
            {
                EntityIdFilter = EntityIdFilter.Like(document.Id), // we create new document with unique id, so using this filter we should find single event from step 2
            };

            var eventSubscriptions = await testContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            var subscriptionToDelete = eventSubscriptions.Data.First();
            Console.WriteLine($"✓ Found event subscription with ID: {subscriptionToDelete.Id}");
            Console.WriteLine($"  - Event Type: {subscriptionToDelete.Event}");
            Console.WriteLine($"  - Entity ID: {subscriptionToDelete.EntityUid}");
            Console.WriteLine($"  - Callback URL: {subscriptionToDelete.JsonAttributes.CallbackUrl}");
            Console.WriteLine($"  - Created: {subscriptionToDelete.Created}");
            Console.WriteLine($"  - Active: {subscriptionToDelete.Active}");

            // Delete the event subscription
            try
            {
                await testContext.Events
                    .DeleteEventSubscriptionAsync(subscriptionToDelete.Id)
                    .ConfigureAwait(false);

                Console.WriteLine($"✓ Successfully deleted event subscription: {subscriptionToDelete.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to delete event subscription: {ex.Message}");
                throw;
            }

            // Verify the deletion by trying to list the subscription again
            var remainingSubscriptions = await testContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            var deletedSubscription = remainingSubscriptions.Data
                .FirstOrDefault(s => s.Id == subscriptionToDelete.Id);

            if (deletedSubscription == null)
            {
                Console.WriteLine("✓ Verified: Event subscription has been successfully deleted");
            }
            else
            {
                Console.WriteLine("⚠️  Warning: Event subscription still exists after deletion attempt");
            }

            // Clean up
            DeleteTestDocument(document.Id);
        }

        /// <summary>
        /// Demonstrates error handling when trying to delete a non-existent event subscription.
        /// </summary>
        [TestMethod]
        public async Task DeleteNonExistentEventSubscriptionExampleAsync()
        {
            Console.WriteLine("=== Delete Non-Existent Event Subscription Example ===");

            // Try to delete an event subscription that doesn't exist (valid format but doesn't exist)
            var nonExistentId = "1234567890abcdef1234567890abcdef12345678"; 

            try
            {
                await testContext.Events
                    .DeleteEventSubscriptionAsync(nonExistentId)
                    .ConfigureAwait(false);

                Console.WriteLine("❌ Unexpected: Delete operation succeeded for non-existent ID");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✓ Expected exception caught: {ex.Message}");
                Console.WriteLine($"  Exception type: {ex.GetType().Name}");
            }
        }
    }
}
