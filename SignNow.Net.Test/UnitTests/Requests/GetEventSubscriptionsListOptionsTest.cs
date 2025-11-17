using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace UnitTests.Requests
{
    [TestClass]
    public class GetEventSubscriptionsListOptionsTest
    {
        [DataTestMethod]
        [DataRow(null, null, null, "")]
        [DataRow(null, null, true, "include_event_count=true")]
        [DataRow(2, 50, null, "page=2&per_page=50")]
        [DataRow(2, 5, false, "page=2&per_page=5&include_event_count=false")]
        public void ToQueryString_Page_PerPage_IncludeEventCount_ReturnsCorrectFormat(int? page, int? perPage, bool? includeEventCount, string expectedQuery)
        {
            var options = new GetEventSubscriptionsListOptions
            {
                Page = page,
                PerPage = perPage,
                IncludeEventCount = includeEventCount
            };

            Assert.AreEqual(expectedQuery, options.ToQueryString());
        }

        [DataTestMethod]
        [DataRow(null, null, null, "")]
        [DataRow(SortOrder.Descending, null, null, "sort[application]=desc")]
        [DataRow(null, SortOrder.Descending, null, "sort[created]=desc")]
        [DataRow(null, null, SortOrder.Descending, "sort[event]=desc")]
        [DataRow(SortOrder.Ascending, SortOrder.Ascending, SortOrder.Ascending, "sort[application]=asc&sort[created]=asc&sort[event]=asc")]
        public void ToQueryString_Sort_ReturnsCorrectFormat(SortOrder? sortByApplication, SortOrder? sortByCreated, SortOrder? sortByEvent, string expectedQuery)
        {
            var options = new GetEventSubscriptionsListOptions
            {
                SortByApplication = sortByApplication,
                SortByCreated = sortByCreated,
                SortByEvent = sortByEvent
            };

            Assert.AreEqual(expectedQuery, options.ToQueryString());
        }

        #region ApplicationFilter Tests
        static IEnumerable<object[]> FilterDataProvider()
        {
            yield return new object[] { ApplicationFilter.In(), "filters=[{\"application\":{\"type\": \"in\", \"value\":[]}}]" };
            yield return new object[] { ApplicationFilter.In(""), "filters=[{\"application\":{\"type\": \"in\", \"value\":[\"\"]}}]" };
            yield return new object[] { ApplicationFilter.In(null, "app2"), "filters=[{\"application\":{\"type\": \"in\", \"value\":[\"\", \"app2\"]}}]" };
            yield return new object[] { ApplicationFilter.In("app1", "app2"), "filters=[{\"application\":{\"type\": \"in\", \"value\":[\"app1\", \"app2\"]}}]" };
        }
        [DataTestMethod]
        [DynamicData(nameof(FilterDataProvider), DynamicDataSourceType.Method)]
        public void ToQueryString_ApplicationFilters_ReturnsCorrectFormat(ApplicationFilter applicationFilter, string expectedQuery)
        {
            var options = new GetEventSubscriptionsListOptions
            {
                ApplicationFilter = applicationFilter,
            };

            Assert.AreEqual(expectedQuery, options.ToQueryString());
        }

        [TestMethod]
        public void ApplicationFilters_IsNull_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => ApplicationFilter.In(null));
        }
        #endregion

        #region DateFilter Tests
        [TestMethod]
        public void ToQueryString_WithDateRangeFilter_ReturnsCorrectFormat()
        {
            var from = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var to = new DateTime(2023, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            var options = new GetEventSubscriptionsListOptions
            {
                DateFilter = DateRangeFilter.Between(from, to)
            };

            var fromTimestamp = new DateTimeOffset(from).ToUnixTimeSeconds();
            var toTimestamp = new DateTimeOffset(to).ToUnixTimeSeconds();
            var expected = $"filters=[{{\"date\":{{\"type\": \"between\", \"value\":[{fromTimestamp}, {toTimestamp}]}}}}]";

            Assert.AreEqual(expected, options.ToQueryString());
        }

        [TestMethod]
        public void Between_WithFromDateGreaterThanToDate_NoException() // current SignNow API behaviour
        {
            var fromDate = new DateTime(2023, 12, 31, 23, 59, 59, DateTimeKind.Utc);
            var toDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var options = new GetEventSubscriptionsListOptions
            {
                DateFilter = DateRangeFilter.Between(fromDate, toDate)
            };

            var fromTimestamp = new DateTimeOffset(fromDate).ToUnixTimeSeconds();
            var toTimestamp = new DateTimeOffset(toDate).ToUnixTimeSeconds();
            Assert.AreEqual(
                $"filters=[{{\"date\":{{\"type\": \"between\", \"value\":[{fromTimestamp}, {toTimestamp}]}}}}]",
                options.ToQueryString()
            );
        }
        #endregion

        #region EntityIdFilter Tests
        [TestMethod]
        [DataRow("")]
        [DataRow("abcd")]
        public void ToQueryString_EntitytIdFilterDataProviderFilters_ReturnsCorrectFormat(string likeExpression)
        {
            var options = new GetEventSubscriptionsListOptions
            {
                EntityIdFilter = EntityIdFilter.Like(likeExpression)
            };

            Assert.AreEqual($"filters=[{{\"entity_id\":{{\"type\": \"like\", \"value\":\"{likeExpression}\"}}}}]", options.ToQueryString());
        }

        [TestMethod]
        public void EntityIdFilter_IsNull_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => EntityIdFilter.Like(null));
        }
        #endregion

        #region CallbackUrlFilter Tests
        [TestMethod]
        [DataRow("")]
        [DataRow("example.com/webhook")]
        public void CallbackUrlFilter_EntitytIdFilterDataProviderFilters_ReturnsCorrectFormat(string urlExpression)
        {
            var options = new GetEventSubscriptionsListOptions
            {
                CallbackUrlFilter = CallbackUrlFilter.Like(urlExpression)
            };

            Assert.AreEqual($"filters=[{{\"callback_url\":{{\"type\": \"like\", \"value\":\"{urlExpression}\"}}}}]", options.ToQueryString());
        }

        [TestMethod]
        public void CallbackUrlFilter_IsNull_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => CallbackUrlFilter.Like(null));
        }
        #endregion

        #region EventTypeFilter Tests
        [TestMethod]
        public void EventTypeFilter_WithValidEventTypes_ReturnsCorrectFilter()
        {
            var options = new GetEventSubscriptionsListOptions
            {
                EventTypeFilter = EventTypeFilter.In(EventType.DocumentComplete, EventType.DocumentUpdate)
            };

            Assert.AreEqual("filters=[{\"event\":{\"type\": \"in\", \"value\":[\"document.complete\", \"document.update\"]}}]", options.ToQueryString());
        }

        [TestMethod]
        public void EventTypeFilter_WithEmptyArrayOfEventTypes_ReturnsCorrectFilter()
        {
            var options = new GetEventSubscriptionsListOptions
            {
                EventTypeFilter = EventTypeFilter.In()
            };

            Assert.AreEqual("filters=[{\"event\":{\"type\": \"in\", \"value\":[]}}]", options.ToQueryString());
        }

        [TestMethod]
        public void EventTypeFilter_IsNull_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => EventTypeFilter.In(null));
        }
        #endregion

        [TestMethod]
        public void ToQueryString_WithComplexFiltering_ReturnsCorrectFormat()
        {
            var options = new GetEventSubscriptionsListOptions
            {
                Page = 1,
                SortByCreated = SortOrder.Descending,
                ApplicationFilter = ApplicationFilter.In("App1", "App2"),
                EventTypeFilter = EventTypeFilter.In(EventType.DocumentComplete)
            };

            Assert.AreEqual(
                "filters=[{\"application\":{\"type\": \"in\", \"value\":[\"App1\", \"App2\"]}}, {\"event\":{\"type\": \"in\", \"value\":[\"document.complete\"]}}]&sort[created]=desc&page=1",
                options.ToQueryString()
            );
        }
    }
    
}
