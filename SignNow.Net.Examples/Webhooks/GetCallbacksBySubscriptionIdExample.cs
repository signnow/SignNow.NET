using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Examples
{
    [TestClass]
    public class GetCallbacksBySubscriptionIdExample : ExamplesBase
    {
        /// <summary>
        /// Demonstrates how to get webhook callback events for a specific event subscription.
        /// This example shows how to retrieve callback history filtered by subscription ID and analyze webhook delivery results.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/callbacks-info/operations/list-v-2-event-subscription-callbacks-1"/>
        [TestMethod]
        public async Task GetCallbacksBySubscriptionIdAsync()
        {
            var subscriptions = await testContext.Events
                .GetEventSubscriptionsListAsync()
                .ConfigureAwait(false);

            if (subscriptions?.Data == null || subscriptions.Data.Count == 0)
            {
                Console.WriteLine("No event subscriptions found. Please create an event subscription first.");
                return;
            }

            var subscriptionId = subscriptions.Data.First().Id;
            Console.WriteLine($"Using subscription ID: {subscriptionId}");

            // Example 1: Get all callbacks for a specific subscription with default options
            Console.WriteLine("\n=== Example 1: Get all callbacks for specific subscription ===");
            var allCallbacks = await testContext.Events
                .GetCallbacksBySubscriptionIdAsync(subscriptionId)
                .ConfigureAwait(false);

            Console.WriteLine($"Total callbacks found for subscription {subscriptionId}: {allCallbacks.Data.Count}");
            Console.WriteLine($"Current page: {allCallbacks.Meta.Pagination.CurrentPage}");
            Console.WriteLine($"Per page: {allCallbacks.Meta.Pagination.PerPage}");
            Console.WriteLine($"Total pages: {allCallbacks.Meta.Pagination.TotalPages}");
            Console.WriteLine($"Total items: {allCallbacks.Meta.Pagination.Total}");

            // Example 2: Filter successful callbacks for the subscription
            Console.WriteLine($"\n=== Example 2: Get successful callbacks for subscription {subscriptionId} ===");
            var successfulCallbacks = await testContext.Events
                .GetCallbacksBySubscriptionIdAsync(subscriptionId, new GetCallbacksOptions
                {
                    Filters = f => f.Or(
                        f => f.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow),
                        f => f.Code.Between(200, 299),
                        f => f.CallbackUrl.Like("example")
                    ),
                    Sortings = s => s.Code(SortOrder.Descending),
                    PerPage = 10
                })
                .ConfigureAwait(false);

            Console.WriteLine($"Successful callbacks: {successfulCallbacks.Data.Count}");
            foreach (var callback in successfulCallbacks.Data.Take(3))
            {
                Console.WriteLine($"  ID: {callback.Id}");
                Console.WriteLine($"  Subscription ID: {callback.EventSubscriptionId}");
                Console.WriteLine($"  Status Code: {callback.ResponseStatusCode}");
                Console.WriteLine($"  Event: {callback.EventName}");
                Console.WriteLine($"  Entity ID: {callback.EntityId}");
                Console.WriteLine($"  Duration: {callback.Duration:F3}s");
                Console.WriteLine($"  Start Time: {callback.RequestStartTime}");
                Console.WriteLine($"  Callback Url: {callback.CallbackUrl}");
                Console.WriteLine();
            }
        }
    }
}
