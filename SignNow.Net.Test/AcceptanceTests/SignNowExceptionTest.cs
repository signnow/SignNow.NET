using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net;
using SignNow.Net.Exceptions;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    [TestClass]
    public class SignNowExceptionTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public void ExceptionHandlingTest()
        {
            var documentId = "mstestSignNowDotNetSDK000000000000000000";
            var errorMessage = "Unable to find a route to match the URI: document/" + documentId;
            var rawErrorResponse = "{\"404\":\"Unable to find a route to match the URI: document\\/" + documentId + "\"}";

            var exception = Assert.ThrowsException<AggregateException>(
                () => Task.WaitAll(SignNowTestContext.Documents.DeleteDocumentAsync(documentId)));

            Assert.AreEqual(errorMessage, exception.InnerException?.Message);
            Assert.AreEqual(1, exception.InnerExceptions.Count);

            foreach (var ex in exception.InnerExceptions)
            {
                var snException = (SignNowException) ex;
                Assert.AreEqual(errorMessage, snException.Message);
                Assert.AreEqual(HttpStatusCode.NotFound, snException.HttpStatusCode);
                Assert.AreEqual(rawErrorResponse, snException.RawResponse);
                Assert.IsTrue(snException.RawHeaders
                    .ToDictionary(x => x.Key, x => x.Value)
                    .ContainsKey("Connection"));
            }
        }
    }
}
