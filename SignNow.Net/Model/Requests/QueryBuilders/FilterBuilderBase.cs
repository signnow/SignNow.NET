using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using SignNow.Net.Internal.Helpers;

namespace SignNow.Net.Model.Requests.QueryBuilders
{
    /// <summary>
    /// Abstract base class for building filter query conditions.
    /// Provides common functionality for creating AND/OR logical operations and converting filter parameters to query string.
    /// This class serves as the foundation for specific filter builders like CallbackFilterBuilder.
    /// </summary>
    /// <remarks>
    /// This class provides utility methods for:
    /// - Combining multiple filter conditions with logical AND/OR operators
    /// - Converting filter parameters to properly formatted query strings
    /// - Converting enum values to their string representations for API consumption
    /// </remarks>
    public abstract class FilterBuilderBase
    {
        /// <summary>
        /// Combines multiple filter conditions using logical AND operation.
        /// </summary>
        /// <typeparam name="T">The type of filter builder, must inherit from FilterBuilderBase.</typeparam>
        /// <param name="filterBuilder">An array of filter builder functions to combine with AND logic.</param>
        /// <returns>A query string representing the combined AND filter condition.</returns>
        /// <exception cref="ArgumentException">Thrown when no filter builders are provided.</exception>
        /// <remarks>
        /// If only one filter is provided, it returns that filter directly without wrapping in AND logic.
        /// Null filters are automatically excluded from the final condition.
        /// </remarks>
        protected static string And<T>(params Func<T, string>[] filterBuilder) where T : FilterBuilderBase, new()
        {
            if (filterBuilder == null || filterBuilder.Length == 0)
                throw new ArgumentException("At least one filter must be provided for AND operation", nameof(filterBuilder));
            
            if (filterBuilder.Length == 1)
                return filterBuilder[0].Invoke(new T());
            
            var filters = filterBuilder
                .Where(f => f != null)
                .Select(f => f.Invoke(new T()));
            return $"{{\"_AND\": [{string.Join(",", filters)}]}}";
        }

        /// <summary>
        /// Combines multiple filter conditions using logical OR operation.
        /// </summary>
        /// <typeparam name="T">The type of filter builder, must inherit from FilterBuilderBase.</typeparam>
        /// <param name="filterBuilder">An array of filter builder functions to combine with OR logic.</param>
        /// <returns>A query string representing the combined OR filter condition.</returns>
        /// <exception cref="ArgumentException">Thrown when no filter builders are provided.</exception>
        /// <remarks>
        /// If only one filter is provided, it returns that filter directly without wrapping in OR logic.
        /// Null filters are automatically excluded from the final condition.
        /// </remarks>
        protected static string Or<T>(params Func<T, string>[] filterBuilder) where T : FilterBuilderBase, new()
        {
            if (filterBuilder == null || filterBuilder.Length == 0)
                throw new ArgumentException("At least one filter must be provided for OR operation", nameof(filterBuilder));
            
            if (filterBuilder.Length == 1)
                return filterBuilder[0].Invoke(new T());
            
            var filters = filterBuilder
                .Where(f => f != null)
                .Select(f => f.Invoke(new T()));
            return $"{{\"_OR\": [{string.Join(",", filters)}]}}";
        }

        /// <summary>
        /// Creates a condition for a single string value.
        /// </summary>
        /// <param name="param">The parameter name to filter on.</param>
        /// <param name="operation">The filter operation (e.g., "like", "=", "in").</param>
        /// <param name="value">The string value to filter by.</param>
        /// <returns>A query string representing the filter condition with quoted string value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when param or operation is null.</exception>
        /// <remarks>
        /// The resulting query format is: {"param_name":{"type": "operation", "value": "string_value"}}
        /// </remarks>
        protected static string Filter(string param, string operation, string value)
        {
            Guard.ArgumentNotNull(param, nameof(param));
            Guard.ArgumentNotNull(operation, nameof(operation));

            return $"{{\"{param}\":{{\"type\": \"{operation}\", \"value\": \"{value}\"}}}}";
        }

        /// <summary>
        /// Creates a filter condition for an array of string values.
        /// </summary>
        /// <param name="param">The parameter name to filter on.</param>
        /// <param name="operation">The filter operation (e.g., "like", "=", "in").</param>
        /// <param name="values">The array of string values to filter by.</param>
        /// <param name="quoteValues">If false this method could be used for numeric values or other non-string data types</param>
        /// <returns>A query string representing the filter condition with quoted string array values.</returns>
        /// <exception cref="ArgumentNullException">Thrown when param or operation is null.</exception>
        /// <remarks>
        /// The resulting query format is: {"param_name":{"type": "operation", "value": ["value1", "value2"]}}
        /// Each string value in the array is automatically quoted.
        /// </remarks>
        protected static string Filter(string param, string operation, string[] values, bool quoteValues = true)
        {
            Guard.ArgumentNotNull(param, nameof(param));
            Guard.ArgumentNotNull(operation, nameof(operation));

            var arrayValues = quoteValues ? values.Select(v => $"\"{v}\"") : values;
            return $"{{\"{param}\":{{\"type\": \"{operation}\", \"value\": [{string.Join(",", arrayValues)}]}}}}";
        }

        /// <summary>
        /// Converts an array of enum values to their corresponding string representations.
        /// Uses the EnumMemberAttribute values for convertation, so the method works only for enums decorated with it.
        /// </summary>
        /// <param name="enums">The array of enum values to convert.</param>
        /// <returns>An array of string representations of the enum values.</returns>
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
