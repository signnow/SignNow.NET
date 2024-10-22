using System;
using System.Collections.Generic;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.Requests
{
    public class LimitOffsetOptions : IQueryToString
    {
        /// <summary>
        /// Limit of the items in response.
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Offset size for pagination.
        /// </summary>
        public int? Offset { get; set; }

        public string ToQueryString()
        {
            if (Limit == null && Offset == null)
            {
                return String.Empty;
            }

            var options = new List<string>();

            if (Limit != null)
            {
                options.Add($"limit={Limit}");
            }

            if (Offset != null)
            {
                options.Add($"offset={Offset}");
            }

            return String.Join("&", options);
        }
    }
}
