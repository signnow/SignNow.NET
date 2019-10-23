using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Service;
using SignNow.Net.Test;

namespace AcceptanceTests
{
    [TestClass]
    public class SignNowExceptionTest : AuthorizedApiTestBase
    {
        readonly IDocumentService docService;

        public SignNowExceptionTest()
        {
            docService = new DocumentService(ApiBaseUrl, Token);
        }

        [TestMethod]
        public void ExceptionHandlingTest()
        {
            var documentId = "mstestSignNowDotNetSDK";
            var errorMessage = "Unable to find a route to match the URI: document/mstestSignNowDotNetSDK";
            var rawErrorResponse = "{\"404\":\"Unable to find a route to match the URI: document\\/mstestSignNowDotNetSDK\"}";

            var deleteResponse = docService.DeleteDocumentAsync(documentId);

            try
            {
                Task.WaitAll(deleteResponse);
            }
            catch (AggregateException ex)
            {
                foreach (SignNowException snEx in ex.InnerExceptions)
                {
                    Assert.AreEqual(snEx.Message, errorMessage);
                    Assert.AreEqual(snEx.HttpStatusCode, HttpStatusCode.NotFound);

                    Assert.IsNotNull(snEx.RawHeaders);
                    Assert.IsTrue(snEx.RawHeaders.Contains("Access-Control-Allow-Headers"));
                    Assert.AreEqual(rawErrorResponse, snEx.RawResponse);
                }
            }
        }
    }
}
