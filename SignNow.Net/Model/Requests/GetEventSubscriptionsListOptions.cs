using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Options for filtering and sorting event subscriptions list.
    /// </summary>
    public class GetEventSubscriptionsListOptions : IQueryToString
    {
        /// <summary>
        /// Filter results by specific applications.
        /// Use ApplicationFilter.In() to filter by multiple applications
        /// or ApplicationFilter.Equals() to filter by a single application.
        /// </summary>
        public ApplicationFilter ApplicationFilter { get; set; }

        /// <summary>
        /// Filter results by date range (start and end timestamps).
        /// </summary>
        public DateRangeFilter DateFilter { get; set; }

        /// <summary>
        /// Filter of like type for entity IDs.
        /// </summary>
        public EntityIdFilter EntityIdFilter { get; set; }

        /// <summary>
        /// Filter of like type for callback URLs.
        /// </summary>
        public CallbackUrlFilter CallbackUrlFilter { get; set; }

        /// <summary>
        /// Filter results by specific event types.
        /// </summary>
        public EventTypeFilter EventTypeFilter { get; set; }

        /// <summary>
        /// Include the number of events that triggered the webhook in the response.
        /// </summary>
        public bool? IncludeEventCount { get; set; }

        /// <summary>
        /// Page number for pagination. Default is 1.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Qty of intems returned per page.
        /// </summary>
        public int? PerPage { get; set; }

        /// <summary>
        /// Sort results by application name (alphabetically).
        /// </summary>
        public SortOrder? SortByApplication { get; set; }

        /// <summary>
        /// Sort results by creation date.
        /// </summary>
        public SortOrder? SortByCreated { get; set; }

        /// <summary>
        /// Sort results by event type (alphabetically).
        /// </summary>
        public SortOrder? SortByEvent { get; set; }

        /// <summary>
        /// Converts the options to a query string.
        /// </summary>
        /// <returns>Query string representation</returns>
        public string ToQueryString()
        {
            var parameters = new List<string>();
            
            var filters = new List<EventSubscriptionFilter> { ApplicationFilter, DateFilter, EntityIdFilter, CallbackUrlFilter, EventTypeFilter }
                .Where(f => f != null)
                .Select(f => f?.FilterExpression);
            if(filters.Count() > 0)
            {
                parameters.Add($"filters=[{string.Join(", ", filters)}]");
            }

            if (SortByApplication.HasValue)
            {
                parameters.Add(Sort("application", SortByApplication.Value));
            }

            if (SortByCreated.HasValue)
            {
                parameters.Add(Sort("created", SortByCreated.Value));
            }

            if (SortByEvent.HasValue)
            {
                parameters.Add(Sort("event", SortByEvent.Value));
            }

            if (Page.HasValue)
            {
                parameters.Add($"page={Page.Value}");
            }

            if (PerPage.HasValue)
            {
                parameters.Add($"per_page={PerPage.Value}");
            }

            if (IncludeEventCount.HasValue)
            {
                parameters.Add($"include_event_count={IncludeEventCount.Value.ToString().ToLower()}");
            }

            return string.Join("&", parameters);
        }

        private string Sort(string propertyName, SortOrder sortOrder)
        {
            var sortOrderStr = sortOrder == SortOrder.Ascending ? "asc" : "desc";
            return $"sort[{propertyName}]={sortOrderStr}";
        }
    }

    /// <summary>
    /// Base class for event subscription filters.
    /// Provides common functionality for creating query filter strings.
    /// </summary>
    public class EventSubscriptionFilter
    {
        /// <summary>
        /// Returns the string representation of the filter for use in query parameters.
        /// </summary>
        public string FilterExpression { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSubscriptionFilter"/> class.
        /// This class helps create filters in format Filter.In("a", "b"), Filter.Equal("a") etc.
        /// </summary>
        /// <param name="filterExpression">The filter expression string. Builded with help of CreateSingleValueFilter, CreateArrayValueFilter.</param>
        protected EventSubscriptionFilter(string filterExpression)
        {
            FilterExpression = filterExpression;
        }

        /// <summary>
        /// Creates a filter expression for a single value operation.
        /// </summary>
        /// <param name="propertyName">The property name to filter on.</param>
        /// <param name="operation">The filter operation (e.g., "=", "like").</param>
        /// <param name="value">The filter value.</param>
        /// <returns>A formatted filter expression string.</returns>
        protected static string CreateSingleValueFilter(string propertyName, string operation, string value)
            => $"{{\"{propertyName}\":{{\"type\": \"{operation}\", \"value\":\"{value}\"}}}}";

        /// <summary>
        /// Creates a filter expression for an array value operation.
        /// </summary>
        /// <param name="propertyName">The property name to filter on.</param>
        /// <param name="operation">The filter operation (e.g., "in", "between").</param>
        /// <param name="values">The array of filter values.</param>
        /// <returns>A formatted filter expression string.</returns>
        protected static string CreateArrayValueFilter(string propertyName, string operation, string[] values, bool addQuotes = true)
        {
            var quotedValues = addQuotes ? values.Select(v => $"\"{v}\"") : values;
            return $"{{\"{propertyName}\":{{\"type\": \"{operation}\", \"value\":[{string.Join(", ", quotedValues)}]}}}}";
        }
    }

    /// <summary>
    /// Provides filtering capabilities for event subscriptions by application name.
    /// </summary>
    public sealed class ApplicationFilter : EventSubscriptionFilter
    {
        private ApplicationFilter(string filterExpression) : base(filterExpression) { }

        /// <summary>
        /// Creates a filter that matches event subscriptions from any of the specified applications.
        /// </summary>
        /// <param name="applicationNames">The application names to filter by.</param>
        /// <returns>An application filter for the specified names.</returns>
        /// <exception cref="ArgumentException">Thrown when applicationNames is empty or contains null/empty values.</exception>
        public static ApplicationFilter In(params string[] applicationNames)
        {
            return new ApplicationFilter(CreateArrayValueFilter("application", "in", applicationNames));
        }
    }

    /// <summary>
    /// Provides filtering capabilities for event subscriptions by creation date range.
    /// </summary>
    public sealed class DateRangeFilter : EventSubscriptionFilter
    {
        private DateRangeFilter(string filterExpression) : base(filterExpression) { }

        /// <summary>
        /// Creates a filter for event subscriptions created between the specified timestamp range.
        /// </summary>
        /// <param name="fromTimestamp">The start timestamp (Unix seconds, inclusive).</param>
        /// <param name="toTimestamp">The end timestamp (Unix seconds, inclusive).</param>
        /// <returns>A date range filter for the specified period.</returns>
        public static DateRangeFilter Between(long fromTimestamp, long toTimestamp)
        {
            return new DateRangeFilter(CreateArrayValueFilter("date", "between", new[] { fromTimestamp.ToString(), toTimestamp.ToString() }, addQuotes: false));
        }

        /// <summary>
        /// Creates a filter for event subscriptions created between the specified date range.
        /// </summary>
        /// <param name="from">The start date of the range (inclusive).</param>
        /// <param name="to">The end date of the range (inclusive).</param>
        /// <returns>A date range filter for the specified period.</returns>
        public static DateRangeFilter Between(DateTime from, DateTime to)
        {
            var fromTimestamp = ((DateTimeOffset)from).ToUnixTimeSeconds();
            var toTimestamp = ((DateTimeOffset)to).ToUnixTimeSeconds();
            return Between(fromTimestamp, toTimestamp);
        }
    }

    /// <summary>
    /// Provides filtering capabilities for event subscriptions by entity ID pattern matching.
    /// </summary>
    public sealed class EntityIdFilter : EventSubscriptionFilter
    {
        private EntityIdFilter(string filterExpression) : base(filterExpression) { }

        /// <summary>
        /// Creates a filter that matches entity IDs containing the specified pattern.
        /// </summary>
        /// <param name="pattern">The pattern to search for in entity IDs.</param>
        /// <returns>An entity ID filter for the specified pattern.</returns>
        /// <exception cref="ArgumentException">Thrown when pattern is null.</exception>
        public static EntityIdFilter Like(string pattern)
        {
            if (pattern == null)
                throw new ArgumentException("Pattern cannot be null.", nameof(pattern));

            return new EntityIdFilter(CreateSingleValueFilter("entity_id", "like", pattern));
        }
    }

    /// <summary>
    /// Provides filtering capabilities for event subscriptions by callback URL pattern matching.
    /// </summary>
    public sealed class CallbackUrlFilter : EventSubscriptionFilter
    {
        private CallbackUrlFilter(string filterExpression) : base(filterExpression) { }

        /// <summary>
        /// Creates a filter that matches callback URLs containing the specified pattern.
        /// </summary>
        /// <param name="urlPattern">The URL pattern to search for in callback URLs.</param>
        /// <returns>A callback URL filter for the specified pattern.</returns>
        /// <exception cref="ArgumentException">Thrown when urlPattern is null.</exception>
        public static CallbackUrlFilter Like(string urlPattern)
        {
            if (urlPattern == null)
                throw new ArgumentException("URL pattern cannot be null.", nameof(urlPattern));

            return new CallbackUrlFilter(CreateSingleValueFilter("callback_url", "like", urlPattern));
        }
    }

    /// <summary>
    /// Provides filtering capabilities for event subscriptions by event type.
    /// </summary>
    public sealed class EventTypeFilter : EventSubscriptionFilter
    {
        private EventTypeFilter(string filterExpression) : base(filterExpression) { }

        /// <summary>
        /// Creates a filter that matches any of the specified event types.
        /// </summary>
        /// <param name="eventTypes">The event types to filter by.</param>
        /// <returns>An event type filter for the specified types.</returns>
        /// <exception cref="ArgumentException">Thrown when eventTypes is null.</exception>
        public static EventTypeFilter In(params EventType[] eventTypes)
        {
            if (eventTypes == null)
                throw new ArgumentException("EventTypes could not be null.", nameof(eventTypes));

            var enumValues = eventTypes.Select(eventType =>
            {
                var enumValueInfo = eventType.GetType().GetMember(eventType.ToString()).First();
                return enumValueInfo.GetCustomAttribute<EnumMemberAttribute>().Value;
            });

            return new EventTypeFilter(CreateArrayValueFilter("event", "in", enumValues.ToArray()));
        }
    }

}
