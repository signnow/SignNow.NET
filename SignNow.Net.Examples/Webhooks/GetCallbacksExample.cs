using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Examples
{
    [TestClass]
    public class GetCallbacksExample : ExamplesBase
    {
        /// <summary>
        /// Demonstrates how to get a list of webhook callback events with various filtering and sorting options.
        /// This example shows how to retrieve callback history and analyze webhook delivery results using the current SDK API.
        /// </summary>
        /// <see cref="https://docs.signnow.com/docs/signnow/reference/operations/get-v2-event-subscriptions-callbacks"/>
        [TestMethod]
        public async Task GetCallbacksAsync()
        {
            // Example 1: Get all callbacks with default options
            Console.WriteLine("=== Example 1: Get all callbacks with default pagination ===");
            var allCallbacks = await testContext.Events
                .GetCallbacksAsync()
                .ConfigureAwait(false);

            Console.WriteLine($"Total callbacks found: {allCallbacks.Data.Count}");
            Console.WriteLine($"Current page: {allCallbacks.Meta.Pagination.CurrentPage}");
            Console.WriteLine($"Per page: {allCallbacks.Meta.Pagination.PerPage}");
            Console.WriteLine($"Total pages: {allCallbacks.Meta.Pagination.TotalPages}");
            Console.WriteLine($"Total items: {allCallbacks.Meta.Pagination.Total}");

            // Example 2: Filter by successful callbacks using fluent filter builder
            Console.WriteLine("\n=== Example 2: Filter successful callbacks (HTTP 2xx status codes) ===");
            var successfulCallbacks = await testContext.Events
                .GetCallbacksAsync(new GetCallbacksOptions
                {
                    Filters = f => f.Code.Between(200, 299),
                    Sortings = s => s.StartTime(SortOrder.Descending),
                    PerPage = 10
                })
                .ConfigureAwait(false);

            Console.WriteLine($"Successful callbacks: {successfulCallbacks.Data.Count}");
            foreach (var callback in successfulCallbacks.Data.Take(3))
            {
                Console.WriteLine($"  ID: {callback.Id}");
                Console.WriteLine($"  Status Code: {callback.ResponseStatusCode}");
                Console.WriteLine($"  Event: {callback.EventName}");
                Console.WriteLine($"  Entity ID: {callback.EntityId}");
                Console.WriteLine();
            }

            // Example 3: Filter by error responses and callback url using complex filters, sorting by start time and code
            Console.WriteLine("=== Example 3: Filter error callbacks (HTTP 4xx and 5xx) ===");
            var errorCallbacks = await testContext.Events
                .GetCallbacksAsync(new GetCallbacksOptions
                {
                    Filters = f => f.And(
                        f => f.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow),
                        f => f.Or(
                            fb => fb.Code.Between(400, 499),
                            fb => fb.Code.Between(500, 599)
                        ),
                        f => f.CallbackUrl.Like("example.com")
                    ),
                    Sortings = s => s
                        .Code(SortOrder.Ascending)
                        .StartTime(SortOrder.Descending),
                    PerPage = 15
                })
                .ConfigureAwait(false);

            Console.WriteLine($"Error callbacks: {errorCallbacks.Data.Count}");
            foreach (var callback in errorCallbacks.Data.Take(3))
            {
                Console.WriteLine($"  ID: {callback.Id}");
                Console.WriteLine($"  Callback Url: {callback.CallbackUrl}");
                Console.WriteLine($"  Status Code: {callback.ResponseStatusCode}");
                Console.WriteLine($"  Response Content: {callback.ResponseContent ?? "N/A"}");
                Console.WriteLine($"  Application: {callback.ApplicationName}");
                Console.WriteLine();
            }
        }
    }
}
