using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace UnitTests
{
    [TestClass]
    public class GetFolderOptionsTest : SignNowTestBase
    {
        [DataTestMethod]
        [DynamicData(nameof(FolderSortProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void BuildSortQuery(string testName, FolderSort order, string expected)
        {
            var sort = new GetFolderOptions { SortBy = order };

            Assert.AreEqual(expected, sort.ToQueryString());
        }

        [DataTestMethod]
        [DynamicData(nameof(FolderFilterProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void BuildFilterQuery(string testName, FolderFilters filter, string expected)
        {
            var options = new GetFolderOptions { Filters = filter };

            Assert.AreEqual(expected, options.ToQueryString());
        }

        [TestMethod]
        public void BuildFolderQuery()
        {
            var queryOptions = new GetFolderOptions
            {
                Filters = new FolderFilters(SigningStatus.Pending),
                SortBy = new FolderSort(SortByDate.Created, SortOrder.Ascending)
            };

            Assert.AreEqual("filters=signing-status&filter-values=pending&sortby=created&order=asc", queryOptions.ToQueryString());
        }

        #region DataProviders

        public static IEnumerable<object[]> FolderSortProvider()
        {
            yield return new object[] { "by Created in Asc order", new FolderSort(SortByDate.Created, SortOrder.Ascending), "sortby=created&order=asc" };
            yield return new object[] { "by Created in Desc order", new FolderSort(SortByDate.Created, SortOrder.Descending), "sortby=created&order=desc" };
            yield return new object[] { "by Updated in Asc order", new FolderSort(SortByDate.Updated, SortOrder.Ascending), "sortby=updated&order=asc" };
            yield return new object[] { "by Updated in Desc order", new FolderSort(SortByDate.Updated, SortOrder.Descending), "sortby=updated&order=desc" };
        }

        public static IEnumerable<object[]> FolderFilterProvider()
        {
            var testDate = new DateTime(2021, 06, 25);
            const string FilterStatus  = "filters=signing-status&filter-values=";
            const string FilterCreated = "filters=document-created&filter-values=";
            const string FilterUpdated = "filters=document-updated&filter-values=";

            yield return new object[]
            {
                $"with signing status {SigningStatus.Pending}", new FolderFilters(SigningStatus.Pending), FilterStatus + "pending"
            };
            yield return new object[]
            {
                $"with signing status {SigningStatus.Signed}", new FolderFilters(SigningStatus.Signed), FilterStatus + "signed"
            };
            yield return new object[]
            {
                $"with signing status {SigningStatus.WaitingForMe}", new FolderFilters(SigningStatus.WaitingForMe), FilterStatus + "waiting-for-me"
            };
            yield return new object[]
            {
                $"with signing status {SigningStatus.WaitingForOthers}", new FolderFilters(SigningStatus.WaitingForOthers), FilterStatus + "waiting-for-others"
            };
            yield return new object[]
            {
                "with document created", new FolderFilters(new DocumentCreatedFilter(testDate)), FilterCreated + "1624568400"
            };
            yield return new object[]
            {
                "with document updated", new FolderFilters(new DocumentUpdatedFilter(testDate)), FilterUpdated + "1624568400"
            };
            yield return new object[]
            {
                "without filters", null, ""
            };
        }

        public static string TestDisplayName(MethodInfo methodInfo, object[] data) =>
            TestUtils.DynamicDataDisplayName(methodInfo, data);

        #endregion
    }
}
