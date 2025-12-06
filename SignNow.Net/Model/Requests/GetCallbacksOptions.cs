using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using SignNow.Net.Interfaces;

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

        public Func<CallbackSortOptionsBuilder, string> Sortings { get; set; }

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
            return "";
        }

    }

    public class CallbackFilterBuilder
    {
        public ApplicationImplementation Application { get; set; } = new ApplicationImplementation();
        public CodeImplementation Code { get; set; } = new CodeImplementation();
        //public int Date { get; set; }
        //public int EntityId { get; set; }
        //public int CallbackUrl { get; set; }
        //public int InitiatorId { get; set; }
        //public int Event { get; set; }
        //public int EventType { get; set; }

        public string And(params Func<CallbackFilterBuilder, string>[] filterBuilder)
        {
            return "";
        }

        public string Or(params Func<CallbackFilterBuilder, string>[] filterBuilder)
        {
            return "";
        }

        public class ApplicationImplementation
        {
            public string In()
            {
                return "";
            }
        }

        public class CodeImplementation
        {
            public string Equal()
            {
                return "";
            }
        }

        // materialize query
        public override string ToString() => "";
    }

    public class CallbackSortOptionsBuilder
    {
        // set of filters, so last one won

        public enum Sorting
        {
            Asc, Desc
        }

        public CallbackSortOptionsBuilder Application(Sorting sort = Sorting.Asc)
        {
            return this;
        }

        public CallbackSortOptionsBuilder Code(Sorting sort = Sorting.Asc)
        {
            return this;
        }

        public CallbackSortOptionsBuilder EndTime(Sorting sort = Sorting.Asc)
        {
            return this;
        }

        public CallbackSortOptionsBuilder StartTime(Sorting sort = Sorting.Asc)
        {
            return this;
        }

        public CallbackSortOptionsBuilder Event(Sorting sort = Sorting.Asc)
        {
            return this;
        }

        // materialize query
        public override string ToString() => "";
    }

}
