using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests.DocumentGroup;
using SignNow.Net.Test.TestData.FakeModels;

namespace SignNow.Net.Test.UnitTests.Requests
{
    [TestClass]
    public class GetDocumentGroupTemplatesRequestTest
    {
        [TestMethod]
        public void GetDocumentGroupTemplatesRequest_SerializationTest()
        {
            var request = new GetDocumentGroupTemplatesRequestFaker().Generate();
            
            Assert.IsTrue(request.Limit >= 1 && request.Limit <= 50);
            Assert.IsTrue(request.Offset >= 0);
        }

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
        public void GetDocumentGroupTemplatesRequest_DefaultValuesTest()
        {
            var request = new GetDocumentGroupTemplatesRequest
            {
                Limit = 5
            };

            Assert.AreEqual(5, request.Limit);
            Assert.AreEqual(0, request.Offset); // Default value
        }
    }
}
