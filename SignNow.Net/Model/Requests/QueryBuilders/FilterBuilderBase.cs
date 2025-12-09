using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using SignNow.Net.Internal.Helpers;

namespace SignNow.Net.Model.Requests.QueryBuilders
{
    public abstract class FilterBuilderBase
    {
        protected string And<T>(params Func<T, string>[] filterBuilder) where T : FilterBuilderBase, new()
        {
            if (filterBuilder == null || filterBuilder.Length == 0)
                throw new ArgumentException("At least one filter must be provided for AND operation", nameof(filterBuilder));
            
            if (filterBuilder.Length == 1)
                return filterBuilder[0].Invoke(new T());
            
            var filters = filterBuilder
                .Select(f => f.Invoke(new T()))
                .Where(f => !string.IsNullOrEmpty(f));
            return $"{{\"_AND\": [{string.Join(",", filters)}]}}";
        }

        protected string Or<T>(params Func<T, string>[] filterBuilder) where T : FilterBuilderBase, new()
        {
            if (filterBuilder == null || filterBuilder.Length == 0)
                throw new ArgumentException("At least one filter must be provided for OR operation", nameof(filterBuilder));
            
            if (filterBuilder.Length == 1)
                return filterBuilder[0].Invoke(new T());
            
            var filters = filterBuilder
                .Select(f => f.Invoke(new T()))
                .Where(f => !string.IsNullOrEmpty(f));
            return $"{{\"_OR\": [{string.Join(",", filters)}]}}";
        }

        protected static string Filter(string param, string operation, string value)
        {
            Guard.ArgumentNotNull(param, nameof(param));
            Guard.ArgumentNotNull(operation, nameof(operation));

            return $"{{\"{param}\":{{\"type\": \"{operation}\", \"value\": \"{value}\"}}}}";
        }

        protected static string Filter(string param, string operation, string[] value)
        {
            Guard.ArgumentNotNull(param, nameof(param));
            Guard.ArgumentNotNull(operation, nameof(operation));

            return $"{{\"{param}\":{{\"type\": \"{operation}\", \"value\": [{string.Join(",", value.Select(v => $"\"{v}\""))}]}}}}";
        }

        protected static string FilterNoQuotes(string param, string operation, string[] value)
        {
            Guard.ArgumentNotNull(param, nameof(param));
            Guard.ArgumentNotNull(operation, nameof(operation));

            return $"{{\"{param}\":{{\"type\": \"{operation}\", \"value\": [{string.Join(", ", value)}]}}}}";
        }

        protected static string[] EnumToStringValues<T>(T[] enums) where T : Enum
        {
            var enumValues = enums.Select(eventType =>
            {
                var enumValueInfo = eventType.GetType().GetMember(eventType.ToString()).FirstOrDefault();
                var enumMemberAttribute = enumValueInfo.GetCustomAttribute<EnumMemberAttribute>();
                return enumMemberAttribute.Value;
            });
            return enumValues.ToArray();
        }
    }

}
