using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Model.Requests.QueryBuilders
{
    /// <summary>
    /// Provides a fluent interface for building callback sorting options.
    /// Allows sorting callbacks by various properties including application name, response codes, timestamps, and events.
    /// Multiple sorting criteria can be chained together for complex sorting requirements.
    /// </summary>
    /// <example>
    /// <code>
    /// var sortBuilder = new CallbackSortOptionsBuilder();
    /// sortBuilder
    ///     .StartTime(SortOrder.Descending)
    ///     .Code(SortOrder.Ascending)
    ///     .Application(SortOrder.Ascending);
    /// </code>
    /// </example>
    public class CallbackSortOptionsBuilder : SortOptionsBuilderBase
    {
        /// <summary>
        /// Sorts callbacks by application name.
        /// </summary>
        public CallbackSortOptionsBuilder Application(SortOrder order = SortOrder.Ascending)
        {
            sorts["application"] = Sort("application", order);
            return this;
        }

        /// <summary>
        /// Sorts callbacks by HTTP response status code.
        /// </summary>
        public CallbackSortOptionsBuilder Code(SortOrder order = SortOrder.Ascending)
        {
            sorts["code"] = Sort("code", order);
            return this;
        }

        /// <summary>
        /// Sorts callbacks by the timestamp when the callback request ended.
        /// </summary>
        public CallbackSortOptionsBuilder EndTime(SortOrder order = SortOrder.Ascending)
        {
            sorts["end_time"] = Sort("end_time", order);
            return this;
        }

        /// <summary>
        /// Sorts callbacks by the timestamp when the callback request started.
        /// This is the default sort field if no sorting is explicitly specified (descending order).
        /// </summary>
        public CallbackSortOptionsBuilder StartTime(SortOrder order = SortOrder.Ascending)
        {
            sorts["start_time"] = Sort("start_time", order);
            return this;
        }

        /// <summary>
        /// Sorts callbacks by event name (e.g., document.complete, user.document.create).
        /// </summary>
        public CallbackSortOptionsBuilder Event(SortOrder order = SortOrder.Ascending)
        {
            sorts["event"] = Sort("event", order);
            return this;
        }

        /// <summary>
        /// Clears all previously configured sorting options.
        /// After calling this method, no sorting criteria will be applied unless new ones are added.
        /// </summary>
        public CallbackSortOptionsBuilder Clear()
        {
            sorts.Clear();
            return this;
        }
    }
}
