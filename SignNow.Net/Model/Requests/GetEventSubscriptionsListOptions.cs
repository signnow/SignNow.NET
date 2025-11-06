using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        /// Page number for pagination. Default is 1.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Sort results by creation date.
        /// </summary>
        public SortOrder? SortByCreated { get; set; }

        /// <summary>
        /// Sort results by event type (alphabetically).
        /// </summary>
        public SortOrder? SortByEvent { get; set; }

        /// <summary>
        /// Sort results by application name (alphabetically).
        /// </summary>
        public SortOrder? SortByApplication { get; set; }

        /// <summary>
        /// Search string for entity IDs and callback URLs.
        /// </summary>
        public string EntityAndCallbackUrlFilter { get; set; }

        /// <summary>
        /// Filter results by date range (start and end timestamps).
        /// </summary>
        public DateRangeFilter DateFilter { get; set; }

        /// <summary>
        /// Filter results by specific event types.
        /// </summary>
        public IReadOnlyList<EventType> EventTypeFilter { get; set; }

        /// <summary>
        /// Filter results by specific applications.
        /// </summary>
        public IReadOnlyList<string> ApplicationFilter { get; set; }

        /// <summary>
        /// Include the number of events that triggered the webhook in the response.
        /// </summary>
        public bool? IncludeEventCount { get; set; }

        /// <summary>
        /// Converts the options to a query string.
        /// </summary>
        /// <returns>Query string representation</returns>
        public string ToQueryString()
        {
            var parameters = new List<string>();

            // Add page parameter
            if (Page.HasValue)
            {
                parameters.Add($"page={Page.Value}");
            }

            // Add sort parameters
            if (SortByCreated.HasValue)
            {
                var sortValue = SortByCreated.Value == SortOrder.Ascending ? "asc" : "desc";
                parameters.Add($"sort[created]={sortValue}");
            }

            if (SortByEvent.HasValue)
            {
                var sortValue = SortByEvent.Value == SortOrder.Ascending ? "asc" : "desc";
                parameters.Add($"sort[event]={sortValue}");
            }

            if (SortByApplication.HasValue)
            {
                var sortValue = SortByApplication.Value == SortOrder.Ascending ? "asc" : "desc";
                parameters.Add($"sort[application]={sortValue}");
            }

            // Add include_event_count parameter
            if (IncludeEventCount.HasValue)
            {
                parameters.Add($"include_event_count={IncludeEventCount.Value.ToString().ToLowerInvariant()}");
            }

            // Add filters (these require URL encoding and complex JSON structure)
            var filters = BuildFilters();
            if (!string.IsNullOrEmpty(filters))
            {
                parameters.Add($"filters={Uri.EscapeDataString(filters)}");
            }

            return string.Join("&", parameters);
        }

        private string BuildFilters()
        {
            var filterConditions = new List<object>();

            // Entity ID and Callback URL filter using OR condition
            if (!string.IsNullOrEmpty(EntityAndCallbackUrlFilter))
            {
                filterConditions.Add(new
                {
                    _OR = new object[]
                    {
                        new { entity_id = new { type = "like", value = EntityAndCallbackUrlFilter } },
                        new { callback_url = new { type = "like", value = EntityAndCallbackUrlFilter } }
                    }
                });
            }

            // Date filter
            if (DateFilter != null)
            {
                filterConditions.Add(new
                {
                    date = new
                    {
                        type = "between",
                        value = new[] { DateFilter.StartTimestamp, DateFilter.EndTimestamp }
                    }
                });
            }

            // Event type filter
            if (EventTypeFilter != null && EventTypeFilter.Count > 0)
            {
                var eventTypeValues = EventTypeFilter.Select(eventType => 
                    GetEventTypeApiValue(eventType)).ToArray();

                filterConditions.Add(new
                {
                    @event = new
                    {
                        type = "in",
                        value = eventTypeValues
                    }
                });
            }

            // Application filter
            if (ApplicationFilter != null && ApplicationFilter.Count > 0)
            {
                filterConditions.Add(new
                {
                    application = new
                    {
                        type = "in",
                        value = ApplicationFilter.ToArray()
                    }
                });
            }

            if (filterConditions.Count == 0)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(filterConditions);
        }

        private static string GetEventTypeApiValue(EventType eventType)
        {
            var field = eventType.GetType().GetField(eventType.ToString());
            var attribute = field?.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .FirstOrDefault();
            
            return attribute?.Value ?? eventType.ToString().ToLowerInvariant();
        }
    }

    /// <summary>
    /// Date range filter for event subscriptions.
    /// </summary>
    public class DateRangeFilter
    {
        /// <summary>
        /// Start timestamp (Unix timestamp).
        /// </summary>
        public long StartTimestamp { get; set; }

        /// <summary>
        /// End timestamp (Unix timestamp).
        /// </summary>
        public long EndTimestamp { get; set; }

        /// <summary>
        /// Initializes a new instance of DateRangeFilter with DateTime values.
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        public DateRangeFilter(DateTime startDate, DateTime endDate)
        {
            StartTimestamp = ((DateTimeOffset)startDate).ToUnixTimeSeconds();
            EndTimestamp = ((DateTimeOffset)endDate).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Initializes a new instance of DateRangeFilter with Unix timestamps.
        /// </summary>
        /// <param name="startTimestamp">Start timestamp</param>
        /// <param name="endTimestamp">End timestamp</param>
        public DateRangeFilter(long startTimestamp, long endTimestamp)
        {
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
        }
    }
}