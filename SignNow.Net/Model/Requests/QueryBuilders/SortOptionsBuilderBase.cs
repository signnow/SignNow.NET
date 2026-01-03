using System.Collections.Generic;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Model.Requests.QueryBuilders
{
    /// <summary>
    /// Abstract base class for building sorting options in query requests.
    /// Provides common functionality for creating sort parameters and converting them to query string format.
    /// This class serves as the foundation for specific sort builders like CallbackSortOptionsBuilder.
    /// </summary>
    public abstract class SortOptionsBuilderBase
    {
        protected Dictionary<string, string> sorts = new Dictionary<string, string>();

        /// <summary>
        /// Converts all configured sort options to a query string format.
        /// Multiple sort parameters are joined with '&amp;' characters for use in HTTP query strings.
        /// </summary>
        public override string ToString() => string.Join("&", sorts.Values);

        /// <summary>
        /// Creates a formatted sort parameter string for the specified property and sort order.
        /// </summary>
        /// <param name="propertyName">The name of the property to sort by.</param>
        /// <param name="sortOrder">The sort order (ascending or descending).</param>
        protected string Sort(string propertyName, SortOrder sortOrder)
        {
            var sortOrderStr = sortOrder == SortOrder.Ascending ? "asc" : "desc";
            return $"sort[{propertyName}]={sortOrderStr}";
        }
    }
}
