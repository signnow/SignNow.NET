using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var parameters = new List<string>();

            if (Filters != null)
            {
                parameters.Add($"filters=[{Filters.Invoke(new CallbackFilterBuilder())}]");
            }

            // sorts

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

    public class FilterBuilderBase
    {
        protected string And<T>(params Func<T, string>[] filterBuilder) where T : new()
        {
            var res = filterBuilder.Select(b => b.Invoke(new T()));
            return $"{{\"_AND\": [{string.Join(",", res)}]}}";
        }

        public string Or<T>(params Func<T, string>[] filterBuilder) where T : new()
        {
            var res = filterBuilder.Select(b => b.Invoke(new T()));
            return $"{{\"_OR\": [{string.Join(",", res)}]}}";
        }

        protected static string Filter(string param, string operation, string value)
            => $"{{\"{param}\":{{\"type\": \"{operation}\", \"value\": \"{value}\"}}}}";

        protected static string Filter(string param, string operation, string[] value)
            => $"{{\"{param}\":{{\"type\": \"{operation}\", \"value\": [{string.Join(",", value.Select(v => $"\"{v}\""))}]}}}}";

        protected static string FilterNoQuotes(string param, string operation, string[] value)
            => $"{{\"{param}\":{{\"type\": \"{operation}\", \"value\": [{string.Join(", ", value)}]}}}}";

        protected static string[] EnumToStringValues<T>(T[] enums) where T : Enum
        {
            var enumValues = enums.Select(eventType =>
            {
                var enumValueInfo = eventType.GetType().GetMember(eventType.ToString()).First();
                return enumValueInfo.GetCustomAttribute<EnumMemberAttribute>().Value;
            });
            return enumValues.ToArray();
        }
    }

    public class CallbackFilterBuilder : FilterBuilderBase
    {
        public ApplicationImplementation Application { get; set; } = new ApplicationImplementation();
        public CodeImplementation Code { get; set; } = new CodeImplementation();
        public DateImplementation Date { get; set; } = new DateImplementation();
        public EntityIdImplementation EntityId { get; set; } = new EntityIdImplementation();
        public CallbackUrlImplementation CallbackUrl { get; set; } = new CallbackUrlImplementation();
        public InitiatorIdImplementation InitiatorId { get; set; } = new InitiatorIdImplementation();
        public EventImplementation Event { get; set; } = new EventImplementation();
        public EventTypeImplementation EventType { get; set; } = new EventTypeImplementation();

        public string And(params Func<CallbackFilterBuilder, string>[] filterBuilder)
            => base.And<CallbackFilterBuilder>(filterBuilder);

        public string Or(params Func<CallbackFilterBuilder, string>[] filterBuilder)
            => base.Or<CallbackFilterBuilder>(filterBuilder);

        public class ApplicationImplementation
        {
            public string In(params string[] ids) => Filter("application", "in", ids);
        }

        public class CodeImplementation
        {
            public string Between(int from, int to) => FilterNoQuotes("code", "between", new[] { from.ToString(), to.ToString()});
        }

        public class DateImplementation
        {
            public string Between(long from, long to) => FilterNoQuotes("date", "between", new[] { from.ToString(), to.ToString()});

            public string Between(DateTime from, DateTime to) => FilterNoQuotes("date", "between", new[] {
                ((DateTimeOffset)from).ToUnixTimeSeconds().ToString(), ((DateTimeOffset)to).ToUnixTimeSeconds().ToString()
            });
        }

        public class EntityIdImplementation
        {
            public string Like(string value) => Filter("entity_id", "like", value);
        }

        public class CallbackUrlImplementation
        {
            public string Like(string value) => Filter("callback_url", "like", value);
        }

        public class InitiatorIdImplementation
        {
            public string Like(string value) => Filter("initiator_id", "like", value);
        }

        public class EventImplementation
        {
            public string In(params EventType[] events) => Filter("event", "in", EnumToStringValues(events));
        }

        public class EventTypeImplementation
        {
            public string In(params EventSubscriptionEntityType[] events) => Filter("event_type", "in", EnumToStringValues(events));
        }
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
