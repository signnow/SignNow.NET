using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using UnitTests;

namespace AcceptanceTests
{
    [TestClass]
    public class SignNowExceptionTest : AuthorizedApiTestBase
    {
        [TestMethod]
        public async Task ExceptionHandlingTest()
        {
            var documentId = "mstestSignNowDotNetSDK000000000000000000";
            var errorMessage = "Unable to find a route to match the URI: document/" + documentId;
            var rawErrorResponse = "{\"404\":\"Unable to find a route to match the URI: document\\/" + documentId + "\"}";

            var exception = await Assert.ThrowsExceptionAsync<SignNowException>(
                async () => await SignNowTestContext.Documents
                    .DeleteDocumentAsync(documentId)
                    .ConfigureAwait(false))
                .ConfigureAwait(false);

            Assert.AreEqual(errorMessage, exception.Message);
            Assert.AreEqual(HttpStatusCode.NotFound, exception.HttpStatusCode);
            Assert.AreEqual(rawErrorResponse, exception.RawResponse);
            Assert.IsTrue(exception.RawHeaders
                .ToDictionary(x => x.Key, x => x.Value)
                .ContainsKey("Connection"));
        }
    }
}
