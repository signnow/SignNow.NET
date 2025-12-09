using System;

namespace SignNow.Net.Model.Requests.QueryBuilders
{
    public class CallbackFilterBuilder : FilterBuilderBase
    {
        public ApplicationImplementation Application { get; private set; } = new ApplicationImplementation();
        public CodeImplementation Code { get; private set; } = new CodeImplementation();
        public DateImplementation Date { get; private set; } = new DateImplementation();
        public EntityIdImplementation EntityId { get; private set; } = new EntityIdImplementation();
        public CallbackUrlImplementation CallbackUrl { get; private set; } = new CallbackUrlImplementation();
        public InitiatorIdImplementation InitiatorId { get; private set; } = new InitiatorIdImplementation();
        public EventImplementation Event { get; private set; } = new EventImplementation();
        public EventTypeImplementation EventType { get; private set; } = new EventTypeImplementation();

        public string And(params Func<CallbackFilterBuilder, string>[] filterBuilder)
            => base.And(filterBuilder);

        public string Or(params Func<CallbackFilterBuilder, string>[] filterBuilder)
            => base.Or(filterBuilder);

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

}
