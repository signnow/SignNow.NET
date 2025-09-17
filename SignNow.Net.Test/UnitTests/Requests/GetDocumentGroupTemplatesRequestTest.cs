using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Test.TestData.FakeModels;
using System.Collections.Generic;

namespace SignNow.Net.Test.UnitTests.Requests
{
    [TestClass]
    public class GetDocumentGroupTemplatesRequestTest
    {

        [DataTestMethod]
        [DataRow(10, 5, "limit=10&offset=5", DisplayName = "Both limit and offset")]
        [DataRow(25, 0, "limit=25&offset=0", DisplayName = "Limit with zero offset")]
        [DataRow(5, 10, "limit=5&offset=10", DisplayName = "Small limit with offset")]
        public void GetDocumentGroupTemplatesRequest_LimitAndOffsetTest(int limit, int offset, string expectedQueryString)
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = limit,
                Offset = offset
            };

            var queryString = request.ToQueryString();
            
            Assert.AreEqual(expectedQueryString, queryString);
        }

        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_OnlyLimitTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 5
            };

            var queryString = request.ToQueryString();
            
            Assert.AreEqual("limit=5", queryString);
            Assert.IsFalse(queryString.Contains("offset"));
        }


        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_EmptyTest()
        {
            var request = new GetDocumentGroupTemplatesRequest();

            var queryString = request.ToQueryString();
            
            Assert.AreEqual(string.Empty, queryString);
        }

        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_ZeroLimitTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 0
            };

            var queryString = request.ToQueryString();
            
            Assert.AreEqual(string.Empty, queryString);
        }



        [DataTestMethod]
        [DataRow(1, "limit=1", DisplayName = "Minimum limit")]
        [DataRow(50, "limit=50", DisplayName = "Maximum limit")]
        [DataRow(25, "limit=25", DisplayName = "Valid range limit")]
        [DataRow(100, "limit=100", DisplayName = "Large limit")]
        public void GetDocumentGroupTemplatesRequest_LimitTest(int limit, string expectedQueryString)
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = limit
            };

            var queryString = request.ToQueryString();
            
            Assert.AreEqual(expectedQueryString, queryString);
        }

    }
}
