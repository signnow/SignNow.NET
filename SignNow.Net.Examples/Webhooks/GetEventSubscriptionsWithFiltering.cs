using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Examples
{
    [TestClass]
    public partial class GetEventSubscriptionsWithFiltering : ExamplesBase
    {
        /// <summary>
        /// Demonstrates how to get a filtered and sorted list of event subscriptions using the enhanced endpoint.
        /// This example shows various filtering and sorting options available with the new GetEventSubscriptionsListAsync method.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/reference/operations/get-v2-event-subscriptions"/>
        [TestMethod]
        public async Task GetEventSubscriptionsWithFilteringAsync()
        {
            // Example 1: Basic pagination with sorting
            Console.WriteLine("=== Example 1: Basic pagination with sorting ===");
            var basicOptions = new GetEventSubscriptionsListOptions
            {
                Page = 1,
                SortByCreated = SortOrder.Descending,
                IncludeEventCount = true
            };

            var basicResponse = await testContext.Events
                .GetEventSubscriptionsListAsync(basicOptions)
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
                EventTypeFilter = new List<EventType> 
                { 
                    EventType.DocumentComplete,
                    EventType.DocumentUpdate,
                    EventType.UserDocumentCreate
                },
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
                EntityAndCallbackUrlFilter = "signnow", // Search for "signnow" in entity IDs and callback URLs
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

            // Example 4: Date range filtering (last 30 days)
            Console.WriteLine("\n=== Example 4: Date range filtering (last 30 days) ===");
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-30);
            
            var dateOptions = new GetEventSubscriptionsListOptions
            {
                DateFilter = new DateRangeFilter(startDate, endDate),
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

            // Example 5: Complex filtering with multiple criteria
            Console.WriteLine("\n=== Example 5: Complex filtering with multiple criteria ===");
            var complexOptions = new GetEventSubscriptionsListOptions
            {
                Page = 1,
                SortByApplication = SortOrder.Ascending,
                EventTypeFilter = new List<EventType> { EventType.DocumentComplete },
                DateFilter = new DateRangeFilter(startDate, endDate),
                IncludeEventCount = true
            };

            var complexResponse = await testContext.Events
                .GetEventSubscriptionsListAsync(complexOptions)
                .ConfigureAwait(false);

            Console.WriteLine($"Found {complexResponse.Data.Count} document.complete subscriptions from last 30 days");
            foreach (var subscription in complexResponse.Data.Take(3))
            {
                Console.WriteLine($"- App: {subscription.ApplicationName}, Created: {subscription.Created:yyyy-MM-dd}");
            }

            // Example 6: Compare with original method
            Console.WriteLine("\n=== Example 6: Compare with original method ===");
            var originalResponse = await testContext.Events
                .GetEventSubscriptionsAsync(new PagePaginationOptions { Page = 1, PerPage = 5 })
                .ConfigureAwait(false);

            var newResponse = await testContext.Events
                .GetEventSubscriptionsListAsync(new GetEventSubscriptionsListOptions { Page = 1 })
                .ConfigureAwait(false);

            Console.WriteLine($"Original method found: {originalResponse.Data.Count} subscriptions");
            Console.WriteLine($"New enhanced method found: {newResponse.Data.Count} subscriptions");
            Console.WriteLine("The new method provides enhanced filtering and sorting capabilities!");

            Console.WriteLine("\n=== Summary ===");
            Console.WriteLine("The GetEventSubscriptionsListAsync method provides:");
            Console.WriteLine("• Enhanced filtering by event types, date ranges, and text search");
            Console.WriteLine("• Flexible sorting by creation date, event type, or application name");
            Console.WriteLine("• Support for complex query combinations");
            Console.WriteLine("• Better performance with the /v2/event-subscriptions endpoint");
        }
    }
}