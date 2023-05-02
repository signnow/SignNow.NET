using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Requests.GetFolderQuery;

namespace UnitTests.Requests
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

        [DataTestMethod]
        [DynamicData(nameof(FolderOptionsProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void BuildOptionsQuery(string testName, GetFolderOptions options, string expected)
        {
            Assert.AreEqual(expected, options?.ToQueryString());
        }

        [TestMethod]
        public void BuildFolderQuery()
        {
            var queryOptions = new GetFolderOptions
            {
                Filters = new FolderFilters(SigningStatus.Pending),
                SortBy = new FolderSort(SortByParam.Created, SortOrder.Ascending),
                Limit = 200,
                Offset = 10,
                EntityTypes = EntityType.All,
                SubfolderData = SubFolders.DoNotShow,
                WithTeamDocuments = false,
                IncludeDocumentsSubfolder = false,
                ExcludeDocumentsRelations = false
            };

            var expected = "filters=signing-status&filter-values=pending"
                        + "&sortby=created"
                        + "&order=asc"
                        + $"&limit={queryOptions.Limit}"
                        + $"&offset={queryOptions.Offset}"
                        + $"&entity_type={queryOptions.EntityTypes.ToString().ToLower()}"
                        + $"&subfolder-data={(int)queryOptions.SubfolderData}"
                        + $"&with_team_documents={queryOptions.WithTeamDocuments.ToString().ToLower()}"
                        + $"&include_documents_subfolders={queryOptions.IncludeDocumentsSubfolder.ToString().ToLower()}"
                        + $"&exclude_documents_relations={queryOptions.ExcludeDocumentsRelations.ToString().ToLower()}";

            Assert.AreEqual(expected, queryOptions.ToQueryString());
        }

        #region DataProviders

        public static IEnumerable<object[]> FolderSortProvider()
        {
            yield return new object[] { "by Created in Asc order", new FolderSort(SortByParam.Created, SortOrder.Ascending), "sortby=created&order=asc" };
            yield return new object[] { "by Created in Desc order", new FolderSort(SortByParam.Created, SortOrder.Descending), "sortby=created&order=desc" };
            yield return new object[] { "by Updated in Asc order", new FolderSort(SortByParam.Updated, SortOrder.Ascending), "sortby=updated&order=asc" };
            yield return new object[] { "by Updated in Desc order", new FolderSort(SortByParam.Updated, SortOrder.Descending), "sortby=updated&order=desc" };
            yield return new object[] { "by Doc name in Asc order", new FolderSort(SortByParam.DocumentName, SortOrder.Ascending), "sortby=document-name&order=asc" };
            yield return new object[] { "by Doc name in Desc order", new FolderSort(SortByParam.DocumentName, SortOrder.Descending), "sortby=document-name&order=desc" };
        }

        public static IEnumerable<object[]> FolderFilterProvider()
        {
            var testDate = new DateTime(2021, 06, 25, 0, 0, 0, DateTimeKind.Utc);
            const string FilterStatus  = "filters=signing-status&filter-values=";
            const string FilterCreated = "filters=document-created&filter-values=";
            const string FilterUpdated = "filters=document-updated&filter-values=";
            const string FilterHasFields = "filters=has-fields&filter-values=";

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
                $"with signing status {SigningStatus.Canceled}", new FolderFilters(SigningStatus.Canceled), FilterStatus + "canceled"
            };
            yield return new object[]
            {
                $"with signing status {SigningStatus.DoesNotCreated}", new FolderFilters(SigningStatus.DoesNotCreated), FilterStatus + "does-not-created"
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
                "with document created", new FolderFilters(new DocumentCreatedFilter(testDate)), FilterCreated + "1624579200"
            };
            yield return new object[]
            {
                "with document updated", new FolderFilters(new DocumentUpdatedFilter(testDate)), FilterUpdated + "1624579200"
            };
            yield return new object[]
            {
                "with fields and roles", new FolderFilters(HasFieldsAndRoles.Yes), FilterHasFields + "1"
            };
            yield return new object[]
            {
                "without fields and roles", new FolderFilters(HasFieldsAndRoles.No), FilterHasFields + "0"
            };
            yield return new object[]
            {
                "without filters", null, ""
            };
        }

        public static IEnumerable<object[]> FolderOptionsProvider()
        {
            yield return new object[] { "with limit=0", new GetFolderOptions {Limit = 0}, "limit=0" };
            yield return new object[] { "with wrong limit", new GetFolderOptions {Limit = -1}, "" };
            yield return new object[] { "with limit > 100", new GetFolderOptions {Limit = 999}, "limit=100" };
            yield return new object[] { "with offset=0", new GetFolderOptions {Offset = 0}, "offset=0" };
            yield return new object[] { "with offset<0", new GetFolderOptions {Offset = -42}, "offset=0" };
            yield return new object[] { "with offset>0", new GetFolderOptions {Offset = 999}, "offset=999" };
            yield return new object[] { "with subfolders", new GetFolderOptions {SubfolderData = SubFolders.Show}, "subfolder-data=1" };
            yield return new object[] { "without subfolders", new GetFolderOptions {SubfolderData = SubFolders.DoNotShow}, "subfolder-data=0" };
            yield return new object[] { "with team docs", new GetFolderOptions {WithTeamDocuments = true}, "with_team_documents=true" };
            yield return new object[] { "without team docs", new GetFolderOptions {WithTeamDocuments = false}, "with_team_documents=false" };
            yield return new object[] { "include documents subfolder", new GetFolderOptions {IncludeDocumentsSubfolder = true}, "include_documents_subfolders=true" };
            yield return new object[] { "exclude documents subfolder", new GetFolderOptions {IncludeDocumentsSubfolder = false}, "include_documents_subfolders=false" };
            yield return new object[] { "exclude documents relations", new GetFolderOptions {ExcludeDocumentsRelations = true}, "exclude_documents_relations=true" };
            yield return new object[] { "include documents relations", new GetFolderOptions {ExcludeDocumentsRelations = false}, "exclude_documents_relations=false" };
            yield return new object[] { "with entity type all", new GetFolderOptions {EntityTypes = EntityType.All}, "entity_type=all" };
            yield return new object[] { "with entity type document", new GetFolderOptions {EntityTypes = EntityType.Document}, "entity_type=document" };
            yield return new object[] { "with entity type document-group", new GetFolderOptions {EntityTypes = EntityType.DocumentGroup}, "entity_type=document-group" };
        }

        public static string TestDisplayName(MethodInfo methodInfo, object[] data) =>
            TestUtils.DynamicDataDisplayName(methodInfo, data);

        #endregion
    }
}
