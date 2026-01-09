using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Examples
{
    [TestClass]
    public class GetEventSubscriptionsWithFiltering : ExamplesBase
    {
        private string testDocumentId;
        private string eventId;

        [TestInitialize]
        public async Task Initialize()
        {
            var uploadResponse = await testContext.Documents
                .UploadDocumentAsync(File.OpenRead(PdfWithoutFields), "Test.Pdf")
                .ConfigureAwait(false);

            testDocumentId = uploadResponse.Id;

            await testContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentFreeformSigned, testDocumentId, new Uri("https://example.com"))
            ).ConfigureAwait(false);
        }

        /// <summary>
        /// Demonstrates how to get a filtered and sorted list of event subscriptions using the enhanced endpoint.
        /// This example shows various filtering and sorting options available with the GetEventSubscriptionsListAsync method.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/reference/operations/get-v2-event-subscriptions"/>
        [TestMethod]
        public async Task GetEventSubscriptionsWithFilteringAsync()
        {
            // Example 1: Basic pagination with sorting
            Console.WriteLine("=== Example 1: Basic pagination with sorting ===");
            var options = new GetEventSubscriptionsListOptions
            {
                Page = 1,
                SortByCreated = SortOrder.Descending,
                IncludeEventCount = true
            };

            var basicResponse = await testContext.Events
                .GetEventSubscriptionsListAsync(options)
                .ConfigureAwait(false);

            Console.WriteLine($"Total event subscriptions: {basicResponse.Meta.Pagination.Total}");
            Console.WriteLine($"Showing page {basicResponse.Meta.Pagination.CurrentPage} of {basicResponse.Meta.Pagination.TotalPages}");
            
            foreach (var subscription in basicResponse.Data.Take(3)) // Show first 3 for brevity
            {
                Console.WriteLine($"- ID: {subscription.Id}, Event: {subscription.Event}, Created: {subscription.Created:yyyy-MM-dd}");
            }

            // Example 2: Filter by specific event types
            Console.WriteLine("\n=== Example 2: Filter by specific event types ===");
            var eventTypeOptions = new GetEventSubscriptionsListOptions
            {
                EventTypeFilter = EventTypeFilter.In(EventType.DocumentComplete, EventType.DocumentUpdate, EventType.UserDocumentCreate),
                SortByEvent = SortOrder.Ascending,
                Page = 1
            };

            var eventTypeResponse = await testContext.Events
                .GetEventSubscriptionsListAsync(eventTypeOptions)
                .ConfigureAwait(false);

            Console.WriteLine($"Found {eventTypeResponse.Data.Count} subscriptions for document events");
            foreach (var subscription in eventTypeResponse.Data.Take(3))
            {
                Console.WriteLine($"- Event: {subscription.Event}, Application: {subscription.ApplicationName}");
            }

            // Example 3: Search in entity IDs and callback URLs
            Console.WriteLine("\n=== Example 3: Search in entity IDs and callback URLs ===");
            var searchOptions = new GetEventSubscriptionsListOptions
            {
                EntityIdFilter = EntityIdFilter.Like(testDocumentId),
                CallbackUrlFilter = CallbackUrlFilter.Like("https://example.com"),
                SortByCreated = SortOrder.Descending,
                Page = 1
            };

            var searchResponse = await testContext.Events
                .GetEventSubscriptionsListAsync(searchOptions)
                .ConfigureAwait(false);

            Console.WriteLine($"Found {searchResponse.Data.Count} subscriptions matching 'signnow'");
            foreach (var subscription in searchResponse.Data.Take(3))
            {
                Console.WriteLine($"- Callback URL: {subscription.JsonAttributes.CallbackUrl}");
            }
            eventId = searchResponse.Data.First().Id;
            // Example 4: Date range filtering (last 30 days)
            Console.WriteLine("\n=== Example 4: Date range filtering (last 30 days) ===");
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-10);
            
            var dateOptions = new GetEventSubscriptionsListOptions
            {
                DateFilter = DateRangeFilter.Between(startDate, endDate),
                SortByCreated = SortOrder.Descending,
                Page = 1
            };

            var dateResponse = await testContext.Events
                .GetEventSubscriptionsListAsync(dateOptions)
                .ConfigureAwait(false);

            Console.WriteLine($"Found {dateResponse.Data.Count} subscriptions created in the last 30 days");
            foreach (var subscription in dateResponse.Data.Take(3))
            {
                Console.WriteLine($"- Created: {subscription.Created:yyyy-MM-dd HH:mm}, Event: {subscription.Event}");
            }

            Console.WriteLine("\n=== Summary ===");
            Console.WriteLine("The GetEventSubscriptionsListAsync method provides:");
            Console.WriteLine("• Enhanced filtering by event types, date ranges, and text search");
            Console.WriteLine("• Flexible sorting by creation date, event type, or application name");
            Console.WriteLine("• Support for complex query combinations");
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await testContext.Events
                .UnsubscribeEventSubscriptionAsync(eventId)
                .ConfigureAwait(false);

            DeleteTestDocument(testDocumentId);
        }
    }
}
