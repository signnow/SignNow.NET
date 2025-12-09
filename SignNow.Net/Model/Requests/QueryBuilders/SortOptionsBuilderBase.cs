using System.Collections.Generic;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace SignNow.Net.Model.Requests.QueryBuilders
{
    public abstract class SortOptionsBuilderBase
    {
        protected Dictionary<string, string> sorts = new Dictionary<string, string>();

        public override string ToString() => string.Join("&", sorts.Values);

        protected string Sort(string propertyName, SortOrder sortOrder)
        {
            var sortOrderStr = sortOrder == SortOrder.Ascending ? "asc" : "desc";
            return $"sort[{propertyName}]={sortOrderStr}";
        }
    }
}
