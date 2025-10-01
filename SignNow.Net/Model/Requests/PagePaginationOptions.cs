using System;
using System.Collections.Generic;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.Requests
{
    public class PagePaginationOptions : IQueryToString
    {
        public int? Page { get; set; }
        public int? PerPage { get; set; }

        public string ToQueryString()
        {
            if (Page == null && PerPage == null)
            {
                return String.Empty;
            }

            var options = new List<string>();
            if (Page != null)
            {
                options.Add($"page={Page}");
            }
            if (PerPage != null)
            {
                options.Add($"per_page={PerPage}");
            }

            return String.Join("&", options);
        }
    }
}
