using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Model.Requests.QueryBuilders
{
    public class CallbackSortOptionsBuilder : SortOptionsBuilderBase
    {
        public CallbackSortOptionsBuilder Application(SortOrder sort = SortOrder.Ascending)
        {
            sorts["application"] = Sort("application", sort);
            return this;
        }

        public CallbackSortOptionsBuilder Code(SortOrder sort = SortOrder.Ascending)
        {
            sorts["code"] = Sort("code", sort);
            return this;
        }

        public CallbackSortOptionsBuilder EndTime(SortOrder sort = SortOrder.Ascending)
        {
            sorts["end_time"] = Sort("end_time", sort);
            return this;
        }

        public CallbackSortOptionsBuilder StartTime(SortOrder sort = SortOrder.Ascending)
        {
            sorts["start_time"] = Sort("start_time", sort);
            return this;
        }

        public CallbackSortOptionsBuilder Event(SortOrder sort = SortOrder.Ascending)
        {
            sorts["event"] = Sort("event", sort);
            return this;
        }

        /// <summary>
        /// Clears all sorting options.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public CallbackSortOptionsBuilder Clear()
        {
            sorts.Clear();
            return this;
        }
    }
}
