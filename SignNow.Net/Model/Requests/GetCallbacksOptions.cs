using System;
using System.Collections.Generic;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.Requests.QueryBuilders;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Options for filtering and sorting callbacks list.
    /// Supports filtering by entity ID, callback URL, date range, response codes, event types, and applications.
    /// If no sort parameter is specified, results are sorted by start_time in descending order.
    /// </summary>
    public class GetCallbacksOptions : IQueryToString
    {
        public Func<CallbackFilterBuilder, string> Filters { get; set; }

        public Func<CallbackSortOptionsBuilder, CallbackSortOptionsBuilder> Sortings { get; set; }

        // todo: check Page & PerPage
        /// <summary>
        /// Page number for pagination. Default is 1.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Qty of intems returned per page.
        /// </summary>
        public int? PerPage { get; set; }

        /// <summary>
        /// Converts the options to a query string.
        /// </summary>
        /// <returns>Query string representation</returns>
        public string ToQueryString()
        {
            var parameters = new List<string>();

            if (Filters != null)
            {
                parameters.Add($"filters=[{Filters.Invoke(new CallbackFilterBuilder())}]");
            }

            if(Sortings != null)
            {
                parameters.Add(Sortings.Invoke(new CallbackSortOptionsBuilder()).ToString());
            }

            if (Page.HasValue)
            {
                parameters.Add($"page={Page.Value}");
            }

            if (PerPage.HasValue)
            {
                parameters.Add($"per_page={PerPage.Value}");
            }

            return string.Join("&", parameters);
        }

    }

}
