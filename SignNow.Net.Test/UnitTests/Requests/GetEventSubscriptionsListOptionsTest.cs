using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace UnitTests.Requests
{
    [TestClass]
    public class GetEventSubscriptionsListOptionsTest : SignNowTestBase
    {
        [DataTestMethod]
        [DynamicData(nameof(PaginationDataProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void BuildPaginationQuery(string testName, GetEventSubscriptionsListOptions options, string expected)
        {
            Assert.AreEqual(expected, options?.ToQueryString());
        }

        [DataTestMethod]
        [DynamicData(nameof(SortDataProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void BuildSortQuery(string testName, GetEventSubscriptionsListOptions options, string expected)
        {
            Assert.AreEqual(expected, options?.ToQueryString());
        }

        [DataTestMethod]
        [DynamicData(nameof(FilterDataProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void BuildFilterQuery(string testName, GetEventSubscriptionsListOptions options, string expected)
        {
            Assert.AreEqual(expected, options?.ToQueryString());
        }

        [TestMethod]
        public void BuildComplexQuery()
        {
            var options = new GetEventSubscriptionsListOptions
            {
                Page = 2,
                SortByCreated = SortOrder.Descending,
                SortByEvent = SortOrder.Ascending,
                EntityAndCallbackUrlFilter = "test_string",
                DateFilter = new DateRangeFilter(1580719773, 1580720509),
                EventTypeFilter = new List<EventType> { EventType.DocumentComplete, EventType.DocumentUpdate },
                ApplicationFilter = new List<string> { "app1", "app2" },
                IncludeEventCount = true
            };

            var queryString = options.ToQueryString();

            Assert.IsTrue(queryString.Contains("page=2"));
            Assert.IsTrue(queryString.Contains("sort[created]=desc"));
            Assert.IsTrue(queryString.Contains("sort[event]=asc"));
            Assert.IsTrue(queryString.Contains("include_event_count=true"));
            Assert.IsTrue(queryString.Contains("filters="));
        }

        [TestMethod]
        public void EmptyOptions_ReturnsEmptyString()
        {
            var options = new GetEventSubscriptionsListOptions();
            Assert.AreEqual(string.Empty, options.ToQueryString());
        }

        [TestMethod]
        public void NullOptions_ReturnsEmptyString()
        {
            GetEventSubscriptionsListOptions options = null;
            Assert.AreEqual(string.Empty, options?.ToQueryString() ?? string.Empty);
        }

        [TestMethod]
        public void DateRangeFilter_WithDateTimeConstructor()
        {
            var startDate = new DateTime(2021, 6, 25, 10, 30, 0, DateTimeKind.Utc);
            var endDate = new DateTime(2021, 6, 26, 10, 30, 0, DateTimeKind.Utc);
            var filter = new DateRangeFilter(startDate, endDate);

            // Use DateTimeOffset for proper UTC conversion
            var expectedStart = new DateTimeOffset(startDate).ToUnixTimeSeconds();
            var expectedEnd = new DateTimeOffset(endDate).ToUnixTimeSeconds();

            Assert.AreEqual(expectedStart, filter.StartTimestamp);
            Assert.AreEqual(expectedEnd, filter.EndTimestamp);
        }

        [TestMethod]
        public void DateRangeFilter_WithTimestampConstructor()
        {
            var filter = new DateRangeFilter(1580719773, 1580720509);

            Assert.AreEqual(1580719773, filter.StartTimestamp);
            Assert.AreEqual(1580720509, filter.EndTimestamp);
        }

        #region DataProviders

        public static IEnumerable<object[]> PaginationDataProvider()
        {
            yield return new object[] { "with page 1", new GetEventSubscriptionsListOptions { Page = 1 }, "page=1" };
            yield return new object[] { "with page 5", new GetEventSubscriptionsListOptions { Page = 5 }, "page=5" };
            yield return new object[] { "include event count true", new GetEventSubscriptionsListOptions { IncludeEventCount = true }, "include_event_count=true" };
            yield return new object[] { "include event count false", new GetEventSubscriptionsListOptions { IncludeEventCount = false }, "include_event_count=false" };
        }

        public static IEnumerable<object[]> SortDataProvider()
        {
            yield return new object[] { "sort by created ascending", new GetEventSubscriptionsListOptions { SortByCreated = SortOrder.Ascending }, "sort[created]=asc" };
            yield return new object[] { "sort by created descending", new GetEventSubscriptionsListOptions { SortByCreated = SortOrder.Descending }, "sort[created]=desc" };
            yield return new object[] { "sort by event ascending", new GetEventSubscriptionsListOptions { SortByEvent = SortOrder.Ascending }, "sort[event]=asc" };
            yield return new object[] { "sort by event descending", new GetEventSubscriptionsListOptions { SortByEvent = SortOrder.Descending }, "sort[event]=desc" };
            yield return new object[] { "sort by application ascending", new GetEventSubscriptionsListOptions { SortByApplication = SortOrder.Ascending }, "sort[application]=asc" };
            yield return new object[] { "sort by application descending", new GetEventSubscriptionsListOptions { SortByApplication = SortOrder.Descending }, "sort[application]=desc" };
        }

        public static IEnumerable<object[]> FilterDataProvider()
        {
            yield return new object[] 
            { 
                "entity and callback filter", 
                new GetEventSubscriptionsListOptions { EntityAndCallbackUrlFilter = "test123" }, 
                "filters=%5B%7B%22_OR%22%3A%5B%7B%22entity_id%22%3A%7B%22type%22%3A%22like%22%2C%22value%22%3A%22test123%22%7D%7D%2C%7B%22callback_url%22%3A%7B%22type%22%3A%22like%22%2C%22value%22%3A%22test123%22%7D%7D%5D%7D%5D"
            };
            yield return new object[] 
            { 
                "date range filter", 
                new GetEventSubscriptionsListOptions { DateFilter = new DateRangeFilter(1580719773, 1580720509) }, 
                "filters=%5B%7B%22date%22%3A%7B%22type%22%3A%22between%22%2C%22value%22%3A%5B1580719773%2C1580720509%5D%7D%7D%5D"
            };
            yield return new object[] 
            { 
                "event type filter", 
                new GetEventSubscriptionsListOptions { EventTypeFilter = new List<EventType> { EventType.DocumentComplete } }, 
                "filters=%5B%7B%22event%22%3A%7B%22type%22%3A%22in%22%2C%22value%22%3A%5B%22document.complete%22%5D%7D%7D%5D"
            };
            yield return new object[] 
            { 
                "application filter", 
                new GetEventSubscriptionsListOptions { ApplicationFilter = new List<string> { "app1", "app2" } }, 
                "filters=%5B%7B%22application%22%3A%7B%22type%22%3A%22in%22%2C%22value%22%3A%5B%22app1%22%2C%22app2%22%5D%7D%7D%5D"
            };
        }

        public static string TestDisplayName(MethodInfo methodInfo, object[] data) =>
            TestUtils.DynamicDataDisplayName(methodInfo, data);

        #endregion
    }
}