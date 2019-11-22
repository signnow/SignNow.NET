using System;
using System.Collections.Generic;
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
            var documentId = "mstestSignNowDotNetSDK000000000000000000";
            var errorMessage = "Unable to find a route to match the URI: document/" + documentId;
            var rawErrorResponse = "{\"404\":\"Unable to find a route to match the URI: document\\/" + documentId + "\"}";

            var deleteResponse = docService.DeleteDocumentAsync(documentId);

            var expectedHeaders = new List<string>
            {
                "Date",
                "Connection",
                "Server",
                "Access-Control-Allow-Headers",
                "Access-Control-Allow-Origin"
            };


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
                    Assert.AreEqual(rawErrorResponse, snEx.RawResponse);


                    var actual = snEx.RawHeaders.GetEnumerator();
                    var expected = expectedHeaders.GetEnumerator();

                    while (actual.MoveNext() && expected.MoveNext())
                    {
                        Assert.AreEqual(actual.Current.Key, expected.Current);
                    }
                }
            }
        }
    }
}
