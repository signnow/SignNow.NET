using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;
using SignNow.Net.Model.Requests.QueryBuilders;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public partial class EventSubscriptionServiceTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var x = new CallbackFilterBuilder();
            var value = x.Or(
                f => f.Application.In("a", "b"),
                f => f.CallbackUrl.Like("cburl"),
                f => f.Code.Between(100, 110),
                f => f.Date.Between(123, 1234),
                f => f.EntityId.Like("elike"),
                f => f.InitiatorId.Like("ilike"),
                f => f.Event.In(EventType.DocumentComplete, EventType.DocumentFieldInviteReplace),
                f => f.EventType.In(EventSubscriptionEntityType.Document, EventSubscriptionEntityType.Template)
            );

            var sorts = new CallbackSortOptionsBuilder();
            // last definition win
            var sres = sorts.Application()
                .Application(SortOrder.Descending)
                .StartTime(SortOrder.Descending)
                .StartTime(SortOrder.Ascending)
                .Code(SortOrder.Ascending);
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithDefaultOptions()
        {
            // Create an event subscription first to potentially have callbacks
            //await SignNowTestContext.Events.CreateEventSubscriptionAsync(
            //    new CreateEventSubscription(EventType.DocumentFreeformSigned, TestPdfDocumentId, new Uri("https://docs.signnow.com"))
            //).ConfigureAwait(false);

            var response = await SignNowTestContext.Events
                //.GetCallbacksAsync(new GetCallbacksOptions())
                .GetCallbacksAsync(new GetCallbacksOptions()
                {
                    Filters = f => f.Or(
                        f => f.Code.Between(1, 500),
                        f => f.CallbackUrl.Like("example.com")
                    ),
                    Sortings = s => s.StartTime(),
                    PerPage = 50
                })
                .ConfigureAwait(false);

            var res = response.Get<DocumentUpdateEventContent>().ToArray();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.IsNotNull(response.Meta.Pagination);
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithFilters()
        {
            // Create an event subscription to potentially have callbacks
            await SignNowTestContext.Events.CreateEventSubscriptionAsync(
                new CreateEventSubscription(EventType.DocumentFreeformSigned, TestPdfDocumentId, new Uri("https://docs.signnow.com"))
            ).ConfigureAwait(false);

            var options = new GetCallbacksOptions
            {
                //SortByStartTime = SortOrder.Descending,
                //CodeFilter = CodeRangeFilter.Success(),
                //EventFilter = EventFilter.Document(),
                //EventTypeFilter = EventTypeFilter.In(EventType.DocumentFreeformSigned)
            };

            var response = await SignNowTestContext.Events
                .GetCallbacksAsync(options)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);

            // Verify query string generation
            var queryString = options.ToQueryString();
            Assert.IsTrue(queryString.Contains("sort[start_time]=desc"));
            Assert.IsTrue(queryString.Contains("filters="));
        }

        [TestMethod]
        public async Task GetCallbacksAsync_WithSearchFilter()
        {
            var options = new GetCallbacksOptions
            {
                //SearchFilter = TestPdfDocumentId.Substring(0, 8), // Use part of document ID for search
                //SortByCode = SortOrder.Ascending
            };

            var response = await SignNowTestContext.Events
                .GetCallbacksAsync(options)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);

            // Verify query string contains search filter
            var queryString = options.ToQueryString();
            Assert.IsTrue(queryString.Contains("_OR"));
            Assert.IsTrue(queryString.Contains("entity_id"));
            Assert.IsTrue(queryString.Contains("callback_url"));
            Assert.IsTrue(queryString.Contains("initiator_id"));
        }

        [TestMethod]
        public async Task GetCallbacksAsync_VerifyCallbackProperties()
        {
            var response = await SignNowTestContext.Events
                .GetCallbacksAsync()
                .ConfigureAwait(false);

            Assert.IsNotNull(response);

            // If there are any callbacks, verify their properties
            if (response.Data.Any())
            {
                var callback = response.Data.First();
                Assert.IsNotNull(callback.Id);
                //Assert.IsTrue(callback.StartTime > 0);
                //Assert.IsTrue(callback.Code >= 100 && callback.Code < 600);
                //Assert.IsNotNull(callback.Event);
                //Assert.IsNotNull(callback.EventType);

                // Verify computed properties work correctly
                //if (callback.EndTime.HasValue)
                //{
                //    Assert.IsNotNull(callback.EndTimeOffset);
                //    Assert.IsNotNull(callback.Duration);
                //    Assert.IsTrue(callback.Duration.Value.TotalSeconds >= 0);
                //}

                //// Verify status properties
                //if (callback.Code >= 200 && callback.Code < 300)
                //{
                //    Assert.IsTrue(callback.IsSuccessful);
                //    Assert.IsFalse(callback.IsClientError);
                //    Assert.IsFalse(callback.IsServerError);
                //}
                //else if (callback.Code >= 400 && callback.Code < 500)
                //{
                //    Assert.IsFalse(callback.IsSuccessful);
                //    Assert.IsTrue(callback.IsClientError);
                //    Assert.IsFalse(callback.IsServerError);
                //}
                //else if (callback.Code >= 500)
                //{
                //    Assert.IsFalse(callback.IsSuccessful);
                //    Assert.IsFalse(callback.IsClientError);
                //    Assert.IsTrue(callback.IsServerError);
                //}
            }
        }
    }
}
