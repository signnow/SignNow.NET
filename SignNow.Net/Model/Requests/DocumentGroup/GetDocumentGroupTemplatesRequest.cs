using SignNow.Net.Interfaces;
using System.Collections.Generic;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Request model for getting document group templates
    /// </summary>
    public class GetDocumentGroupTemplatesRequest : IQueryToString
    {
        /// <summary>
        /// The number of templates to get (required, 1-50)
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The number of templates to skip from the first one (optional)
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Creates query string parameters for the request
        /// </summary>
        /// <returns>Query string parameters</returns>
        public string ToQueryString()
        {
            // Return empty string if limit is not set (0 or negative)
            if (Limit <= 0)
            {
                return string.Empty;
            }

            var parameters = new List<string>();

            // Always include limit since it's required
            parameters.Add($"limit={Limit}");

            // Only include offset if it's been explicitly set
            if (Offset != null)
            {
                parameters.Add($"offset={Offset}");
            }

            return string.Join("&", parameters);
        }
    }
}
