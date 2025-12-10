using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using UnitTests;

namespace AcceptanceTests
{
    public partial class EventSubscriptionServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task GetCallbacksAsync_WithComplexFilters_ReturnsFilteredResults()
        {
            var options = new GetCallbacksOptions
            {
                Filters = f => f.And(
                    fb => fb.Code.Between(200, 299),
                    fb => fb.Code.Between(400, 499),
                    f => f.And(
                        fb => fb.EventType.In(EventSubscriptionEntityType.User),
                        fb => fb.Event.In(EventType.DocumentComplete, EventType.UserDocumentCreate)
                    )
                ),
                Sortings = s => s
                    .StartTime(SortOrder.Descending)
                    .Code(),
                PerPage = 25
            };

            var response = await SignNowTestContext.Events
                .GetCallbacksAsync(options)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            
            Assert.AreEqual(25, response.Meta.Pagination.PerPage);
            
            // If there are callbacks, verify they match the filter criteria
            foreach (var callback in response.Data)
            {
                var statusCode = callback.ResponseStatusCode;
                Assert.IsTrue((statusCode >= 200 && statusCode <= 299) || (statusCode >= 400 && statusCode <= 499),
                    $"Callback status code {statusCode} should be in range 200-299 or 400-499");
                Assert.AreEqual(EventSubscriptionEntityType.User, callback.EntityType);
                Assert.IsTrue(callback.EventName == EventType.DocumentComplete || callback.EventName == EventType.UserDocumentCreate);
            }

            var queryString = options.ToQueryString();
            Assert.IsTrue(queryString.Contains("filters"));
            Assert.IsTrue(queryString.Contains("sort"));
        }
    }
}
