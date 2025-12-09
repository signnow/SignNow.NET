using System;

namespace SignNow.Net.Model.Requests.QueryBuilders
{
    /// <summary>
    /// Provides a fluent interface for building callback filter queries.
    /// Supports filtering callbacks by various criteria including application, response codes, dates, entity IDs, callback URLs, and event types.
    /// </summary>
    /// <example>
    /// <code>
    /// var filter = new CallbackFilterBuilder();
    /// var query = filter.And(
    ///     f => f.EntityId.Like("doc_123"),
    ///     f => f.Code.Between(200, 299),
    ///     f => f.Date.Between(DateTime.Today.AddDays(-7), DateTime.Today)
    /// );
    /// </code>
    /// </example>
    public class CallbackFilterBuilder : FilterBuilderBase
    {
        /// <summary>
        /// Filter callbacks by application name.
        /// </summary>
        public ApplicationImplementation Application { get; private set; } = new ApplicationImplementation();
        /// <summary>
        /// Filter callbacks by HTTP response status codes.
        /// </summary>
        public CodeImplementation Code { get; private set; } = new CodeImplementation();
        /// <summary>
        /// Filter callbacks by date ranges.
        /// </summary>
        public DateImplementation Date { get; private set; } = new DateImplementation();
        /// <summary>
        /// Filter callbacks by entity identifier patterns.
        /// </summary>
        public EntityIdImplementation EntityId { get; private set; } = new EntityIdImplementation();
        /// <summary>
        /// Filter callbacks by URL patterns.
        /// </summary>
        public CallbackUrlImplementation CallbackUrl { get; private set; } = new CallbackUrlImplementation();
        /// <summary>
        /// Filter callbacks by the user who initiated the action.
        /// </summary>
        public InitiatorIdImplementation InitiatorId { get; private set; } = new InitiatorIdImplementation();
        /// <summary>
        /// Filter callbacks by specific event types.
        /// </summary>
        public EventImplementation Event { get; private set; } = new EventImplementation();
        /// <summary>
        /// Filter callbacks by entity types (document, user, etc.).
        /// </summary>
        public EventTypeImplementation EventType { get; private set; } = new EventTypeImplementation();

        /// <summary>
        /// Combines multiple filter conditions using logical AND operation.
        /// All conditions must be true for a callback to match the filter.
        /// </summary>
        /// <param name="filterBuilders">An array of filter builder functions to combine with AND logic.</param>
        /// <returns>Query string representing the combined AND filter condition.</returns>
        public string And(params Func<CallbackFilterBuilder, string>[] filterBuilders)
            => base.And(filterBuilders);

        /// <summary>
        /// Combines multiple filter conditions using logical OR operation.
        /// At least one condition must be true for a callback to match the filter.
        /// </summary>
        /// <param name="filterBuilders">An array of filter builder functions to combine with OR logic.</param>
        /// <returns>Query string representing the combined OR filter condition.</returns>
        public string Or(params Func<CallbackFilterBuilder, string>[] filterBuilders)
            => base.Or(filterBuilders);

        public class ApplicationImplementation
        {
            /// <summary>
            /// Filters callbacks where the application name matches any of the specified values.
            /// </summary>
            /// <param name="ids">The application ids to filter by.</param>
            /// <returns>A query string for application matching.</returns>
            public string In(params string[] ids) => Filter("application", "in", ids);
        }

        public class CodeImplementation
        {
            /// <summary>
            /// Filters callbacks where the HTTP response status code is within the specified range.
            /// </summary>
            /// <param name="from">The minimum status code.</param>
            /// <param name="to">The maximum status code.</param>
            /// <returns>A query string for status code range matching.</returns>
            /// <example>
            /// <code>
            /// // Filter for successful responses (200-299)
            /// builder.Code.Between(200, 299)
            /// </code>
            /// </example>
            public string Between(int from, int to) => Filter("code", "between", new[] { from.ToString(), to.ToString()}, quoteValues: false);
        }

        /// <summary>
        /// Provides filtering capabilities for callback dates.
        /// </summary>
        public class DateImplementation
        {
            /// <summary>
            /// Filters callbacks where the callback date is within the specified Unix timestamp range.
            /// </summary>
            /// <param name="from">The start Unix timestamp.</param>
            /// <param name="to">The end Unix timestamp.</param>
            /// <returns>A query string for date range matching using Unix timestamps.</returns>
            public string Between(long from, long to) => Filter("date", "between", new[] { from.ToString(), to.ToString()}, quoteValues: false);

            /// <summary>
            /// Filters callbacks where the callback date is within the specified DateTime range.
            /// The DateTime values are automatically converted to Unix timestamps.
            /// </summary>
            /// <param name="from">The start date and time.</param>
            /// <param name="to">The end date and time.</param>
            /// <returns>A query string for date range matching using DateTime objects.</returns>
            /// <example>
            /// <code>
            /// // Filter for callbacks from the last 7 days
            /// builder.Date.Between(DateTime.Today.AddDays(-7), DateTime.Today)
            /// </code>
            /// </example>
            public string Between(DateTime from, DateTime to) => Filter("date", "between", new[] {
                ((DateTimeOffset)from).ToUnixTimeSeconds().ToString(), ((DateTimeOffset)to).ToUnixTimeSeconds().ToString()
            },quoteValues: false);
        }

        public class EntityIdImplementation
        {
            /// <summary>
            /// Filters callbacks where the entity ID contains or matches the specified pattern.
            /// </summary>
            /// <param name="value">The entity ID pattern to search for.</param>
            /// <returns>A query string for entity ID pattern matching.</returns>
            /// <example>
            /// <code>
            /// // Filter for document IDs starting with "doc_"
            /// builder.EntityId.Like("doc_")
            /// </code>
            /// </example>
            public string Like(string value) => Filter("entity_id", "like", value);
        }

        public class CallbackUrlImplementation
        {
            /// <summary>
            /// Filters callbacks where the callback URL contains or matches the specified pattern.
            /// </summary>
            /// <param name="value">The URL pattern to search for.</param>
            /// <returns>A query string for callback URL pattern matching.</returns>
            /// <example>
            /// <code>
            /// // Filter for callbacks to webhook endpoints
            /// builder.CallbackUrl.Like("/webhook")
            /// </code>
            /// </example>
            public string Like(string value) => Filter("callback_url", "like", value);
        }

        public class InitiatorIdImplementation
        {
            /// <summary>
            /// Filters callbacks where the initiator ID contains or matches the specified pattern.
            /// </summary>
            /// <param name="value">The initiator ID pattern to search for.</param>
            /// <returns>A query string for initiator ID pattern matching.</returns>
            /// <example>
            /// <code>
            /// // Filter for callbacks initiated by users with IDs containing "admin"
            /// builder.InitiatorId.Like("admin")
            /// </code>
            /// </example>
            public string Like(string value) => Filter("initiator_id", "like", value);
        }

        public class EventImplementation
        {
            /// <summary>
            /// Filters callbacks where the event type matches any of the specified event types.
            /// </summary>
            /// <param name="events">The event types to filter by.</param>
            /// <returns>A query string for event type matching.</returns>
            /// <example>
            /// <code>
            /// // Filter for document completion and update events
            /// builder.Event.In(EventType.DocumentComplete, EventType.DocumentUpdate)
            /// </code>
            /// </example>
            public string In(params EventType[] events) => Filter("event", "in", EnumToStringValues(events));
        }

        public class EventTypeImplementation
        {
            /// <summary>
            /// Filters callbacks where the entity type matches any of the specified entity types.
            /// </summary>
            /// <param name="events">The entity types to filter by (document, user, document_group, template).</param>
            /// <returns>A query string for entity type matching.</returns>
            /// <example>
            /// <code>
            /// // Filter for document and template related callbacks
            /// builder.EventType.In(EventSubscriptionEntityType.Document, EventSubscriptionEntityType.Template)
            /// </code>
            /// </example>
            public string In(params EventSubscriptionEntityType[] events) => Filter("event_type", "in", EnumToStringValues(events));
        }
    }

}
