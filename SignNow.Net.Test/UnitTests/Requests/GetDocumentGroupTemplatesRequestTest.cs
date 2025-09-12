using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Test.TestData.FakeModels;

namespace SignNow.Net.Test.UnitTests.Requests
{
    [TestClass]
    public class GetDocumentGroupTemplatesRequestTest
    {

        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_ToQueryStringTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 10,
                Offset = 5
            };

            var queryString = request.ToQueryString();
            
            Assert.IsTrue(queryString.Contains("limit=10"));
            Assert.IsTrue(queryString.Contains("offset=5"));
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
        public void GetDocumentGroupTemplatesRequest_OnlyOffsetTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 5,
                Offset = 10
            };

            var queryString = request.ToQueryString();
            
            Assert.IsTrue(queryString.Contains("limit=5"));
            Assert.IsTrue(queryString.Contains("offset=10"));
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

        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_LargeLimitTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 100  // Large value - ToQueryString should still include it
            };

            var queryString = request.ToQueryString();
            
            Assert.AreEqual("limit=100", queryString);
        }

        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_ValidRangeTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 25,
                Offset = 5
            };

            var queryString = request.ToQueryString();
            
            Assert.IsTrue(queryString.Contains("limit=25"));
            Assert.IsTrue(queryString.Contains("offset=5"));
        }

        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_MinimumLimitTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 1
            };

            var queryString = request.ToQueryString();
            
            Assert.AreEqual("limit=1", queryString);
        }

        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_MaximumLimitTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 50
            };

            var queryString = request.ToQueryString();
            
            Assert.AreEqual("limit=50", queryString);
        }

        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_ZeroOffsetTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 10,
                Offset = 0
            };

            var queryString = request.ToQueryString();
            
            Assert.IsTrue(queryString.Contains("limit=10"));
            Assert.IsTrue(queryString.Contains("offset=0"));
        }
    }
}
