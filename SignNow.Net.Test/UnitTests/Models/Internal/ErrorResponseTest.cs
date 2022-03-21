using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Model;

namespace UnitTests
{
    [TestClass]
    public class ErrorResponseTest
    {
        [TestMethod]
        [DynamicData(nameof(ErrorContentProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void ShouldProcessErrorMessage(string testName, string errorContext, string expectedMsg, string expectedCode)
        {
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(errorContext);

            Assert.AreEqual(expectedMsg, errorResponse.GetErrorMessage());
            Assert.AreEqual(expectedCode, errorResponse.GetErrorCode());
        }

        public static IEnumerable<object[]> ErrorContentProvider()
        {
            // Test DisplayName | test object
            yield return new object[] { "Invalid token", @"{""error"":""invalid_token"",""code"": 1537}", "invalid_token", "1537" };
            yield return new object[] { "404", @"{""404"": ""context of error404""}", "context of error404", "" };
            yield return new object[]
            {
                "One error as Array", @"{""errors"":[{""code"":""127"",""message"":""context of single errors item""}]}",
                "context of single errors item", "127"
            };
            yield return new object[]
            {
                "Many errors as Array",
                @"{""errors"":[{""code"":""127"",""message"":""first error""}, {""code"":""128"",""message"":""second error""}]}",
                $"first error{Environment.NewLine}second error{Environment.NewLine}", $"127{Environment.NewLine}128{Environment.NewLine}"
            };
            yield return new object[]
            {
                "Internal Api Error", @"{""error"":[{""code"":""HY000"",""message"":""Internal Api Error""}]}",
                "Internal Api Error", "HY000"
            };
        }

        public static string TestDisplayName(MethodInfo methodInfo, object[] data) =>
            TestUtils.DynamicDataDisplayName(methodInfo, data);
    }
}
