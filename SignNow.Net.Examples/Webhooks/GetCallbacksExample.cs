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
        /// Demonstrates how to get a list of webhook callback events with various filtering and sorting options.
        /// This example shows how to retrieve callback history and analyze webhook delivery results.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/reference/operations/get-v2-event-subscriptions-callbacks"/>
        [TestMethod]
        public async Task GetCallbacksAsync()
        {
            // First, ensure we have a document and event subscription for demonstration
            var uploadResponse = await testContext.Documents
                .UploadDocumentAsync(File.OpenRead(PdfWithoutFields), "CallbackExample.pdf")
                .ConfigureAwait(false);

            var documentId = uploadResponse.Id;

            // Create an event subscription to potentially generate callbacks
            await testContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentFreeformSigned, documentId, new Uri("https://example.com/webhook"))
            ).ConfigureAwait(false);

            // Example 1: Get all callbacks with default sorting (by start_time desc)
            Console.WriteLine("=== Example 1: Get all callbacks ===");
            var allCallbacks = await testContext.Events
                .GetCallbacksAsync()
                .ConfigureAwait(false);

            Console.WriteLine($"Total callbacks found: {allCallbacks.Data.Count}");
            Console.WriteLine($"Current page: {allCallbacks.Meta?.Pagination?.CurrentPage}");
            Console.WriteLine($"Total pages: {allCallbacks.Meta?.Pagination?.TotalPages}");

            // Example 2: Filter by successful callbacks only
            Console.WriteLine("\n=== Example 2: Filter successful callbacks (HTTP 2xx) ===");
            var successfulCallbacks = await testContext.Events
                .GetCallbacksAsync(new GetCallbacksOptions
                {
                    //CodeFilter = CodeRangeFilter.Success(),
                    //SortByStartTime = SortOrder.Descending
                })
                .ConfigureAwait(false);

            Console.WriteLine($"Successful callbacks: {successfulCallbacks.Data.Count}");
            foreach (var callback in successfulCallbacks.Data.Take(3))
            {
                //Console.WriteLine($"  ID: {callback.Id}, Code: {callback.Code}, Event: {callback.Event}");
                //Console.WriteLine($"  Start Time: {callback.StartTimeOffset:yyyy-MM-dd HH:mm:ss} UTC");
                //Console.WriteLine($"  Duration: {callback.Duration?.TotalMilliseconds ?? 0}ms");
                //Console.WriteLine($"  Success: {callback.IsSuccessful}");
            }

            // Example 3: Filter by error responses
            Console.WriteLine("\n=== Example 3: Filter error callbacks (HTTP 4xx and 5xx) ===");
            var errorCallbacks = await testContext.Events
                .GetCallbacksAsync(new GetCallbacksOptions
                {
                    //CodeFilter = new CodeRangeFilter(400, 599), // Client and server errors
                    //SortByCode = SortOrder.Ascending
                })
                .ConfigureAwait(false);

            Console.WriteLine($"Error callbacks: {errorCallbacks.Data.Count}");
            foreach (var callback in errorCallbacks.Data.Take(3))
            {
                //Console.WriteLine($"  ID: {callback.Id}, Code: {callback.Code}");
                //Console.WriteLine($"  Error Message: {callback.ErrorMessage ?? "N/A"}");
                //Console.WriteLine($"  Client Error: {callback.IsClientError}, Server Error: {callback.IsServerError}");
            }

            // Example 4: Search for specific entity or URL
            Console.WriteLine("\n=== Example 4: Search in entity IDs, callback URLs, and initiator IDs ===");
            var searchResults = await testContext.Events
                .GetCallbacksAsync(new GetCallbacksOptions
                {
                    //SearchFilter = documentId.Substring(0, 8), // Search for part of document ID
                    //SortByEvent = SortOrder.Ascending
                })
                .ConfigureAwait(false);

            Console.WriteLine($"Search results: {searchResults.Data.Count}");
            foreach (var callback in searchResults.Data.Take(3))
            {
                Console.WriteLine($"  Entity ID: {callback.EntityId}");
                Console.WriteLine($"  Callback URL: {callback.CallbackUrl}");
                //Console.WriteLine($"  Event Type: {callback.EventType}");
            }

            // Example 5: Filter by specific event types
            Console.WriteLine("\n=== Example 5: Filter by document events ===");
            var documentEvents = await testContext.Events
                .GetCallbacksAsync(new GetCallbacksOptions
                {
                    //EventFilter = EventFilter.Document(),
                    //EventTypeFilter = EventTypeFilter.In(EventType.DocumentFreeformSigned),
                    //SortByApplication = SortOrder.Ascending
                })
                .ConfigureAwait(false);

            Console.WriteLine($"Document event callbacks: {documentEvents.Data.Count}");
            foreach (var callback in documentEvents.Data.Take(3))
            {
                //Console.WriteLine($"  Event: {callback.Event}");
                //Console.WriteLine($"  Event Type: {callback.EventType}");
                //Console.WriteLine($"  Application: {callback.Application}");
            }

            // Example 6: Multiple sorting options
            Console.WriteLine("\n=== Example 6: Sort by multiple criteria ===");
            var sortedCallbacks = await testContext.Events
                .GetCallbacksAsync(new GetCallbacksOptions
                {
                    //SortByEndTime = SortOrder.Descending,
                    //SortByCode = SortOrder.Ascending
                })
                .ConfigureAwait(false);

            Console.WriteLine($"Multi-sorted callbacks: {sortedCallbacks.Data.Count}");

            // Example 7: Analyze callback performance
            Console.WriteLine("\n=== Example 7: Performance Analysis ===");
            if (allCallbacks.Data.Any())
            {
                ////var completedCallbacks = allCallbacks.Data.Where(c => c.Duration.HasValue).ToList();
                //if (completedCallbacks.Any())
                //{
                //    var avgDuration = completedCallbacks.Average(c => c.Duration.Value.TotalMilliseconds);
                //    var maxDuration = completedCallbacks.Max(c => c.Duration.Value.TotalMilliseconds);
                //    var minDuration = completedCallbacks.Min(c => c.Duration.Value.TotalMilliseconds);

                //    Console.WriteLine($"Callback Performance Statistics:");
                //    Console.WriteLine($"  Average Duration: {avgDuration:F2}ms");
                //    Console.WriteLine($"  Max Duration: {maxDuration:F2}ms");
                //    Console.WriteLine($"  Min Duration: {minDuration:F2}ms");
                //}

                //var successRate = (double)allCallbacks.Data.Count(c => c.IsSuccessful) / allCallbacks.Data.Count * 100;
                //Console.WriteLine($"  Success Rate: {successRate:F1}%");
            }

            // Cleanup
            await testContext.Documents.DeleteDocumentAsync(documentId).ConfigureAwait(false);
        }
    }
}
