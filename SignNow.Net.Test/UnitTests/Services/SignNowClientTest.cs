using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Service;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class SignNowClientTest : SignNowTestBase
    {
        public static IEnumerable<object[]> ErrorContentProvider()
        {
            // Test name | errorContext | expected error message
            yield return new object[] { "with property: error", "{'error': 'context of error'}", "context of error" };
            yield return new object[] { "with property: 404",   "{'404': 'context of error404'}", "context of error404" };
            yield return new object[] { "with property: errors[one]",  @"
                {
                'errors': [
                    {
                        'message': 'context of single errors item',
                        'code': 127
                    }]
                }",
                "context of single errors item" };
            yield return new object[]
            {
                "with property: errors[many]", @"
                {
                    'errors': [
                        {
                            'code': 41,
                            'message': 'context of first errors item'
                        },
                        {
                            'code': 42,
                            'message': 'context of second errors item'
                        }
                    ]}",
#if NETFRAMEWORK
                $"context of first errors item{Environment.NewLine}context of second errors item{Environment.NewLine}"
#else
                $"context of first errors item{Environment.NewLine}context of second errors item{Environment.NewLine} (context of first errors item) (context of second errors item)"
#endif
            };
        }

        public static string TestDisplayName(MethodInfo methodInfo, object[] data) =>
            TestUtils.DynamicDataDisplayName(methodInfo, data);

        [DataTestMethod]
        [DynamicData(nameof(ErrorContentProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void ShouldCatchAndProcessAnyOfErrorResponse(string testName, string errorContext, string expectedMessage)
        {
            var signNowClientMock = SignNowClientMock(errorContext, HttpStatusCode.BadRequest);

            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => signNowClientMock.RequestAsync(
                        new PostHttpRequestOptions { RequestUrl = ApiBaseUrl },
                        new HttpContentToObjectAdapter<Token>(new HttpContentToStringAdapter())).Result);

            Assert.AreEqual(expectedMessage, exception.InnerException?.Message);
        }

        [TestMethod]
        public void HandleBrokenJsonResponseWhileDeserialization()
        {
            var signNowClientMock = SignNowClientMock("{Broken response body", HttpStatusCode.BadRequest);

            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => signNowClientMock.RequestAsync(
                        new GetHttpRequestOptions { RequestUrl = ApiBaseUrl },
                        new HttpContentToObjectAdapter<Token>(new HttpContentToStringAdapter())).Result
                    );

            StringAssert.Contains(exception.Message, "One or more errors occurred.");
            StringAssert.Contains(exception.InnerException?.Message, "Newtonsoft.Json.JsonReaderException thrown while parsing Json body from https://api-eval.signnow.com/");
            StringAssert.Contains(exception.InnerException?.InnerException?.Message, "Invalid Json syntax in response");
            StringAssert.Contains(exception.InnerException?.InnerException?.InnerException?.Message, "Invalid character after parsing property name. Expected ':' but got: r. Path '', line 1, position 8.");

            Assert.AreEqual("{Broken response body", ((SignNowException)exception.InnerException)?.RawResponse);
        }

        [TestMethod]
        public void ShouldHandleTimeoutExceptionFromHttpClient()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(
                    new TaskCanceledException("A task was canceled."), TimeSpan.FromSeconds(3))
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object, false);

            var signnowClient = new SignNowClient(httpClient);

            // Override timeout for Client to minimal value
            httpClient.Timeout = TimeSpan.FromSeconds(1);

            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => signnowClient.RequestAsync(
                        new GetHttpRequestOptions { RequestUrl = new Uri(ApiBaseUrl + "user") },
                        new HttpContentToObjectAdapter<Token>(new HttpContentToStringAdapter())).Result);

            var errorMessage = string.Format(CultureInfo.CurrentCulture,
                ExceptionMessages.UnableToProcessRequest,
                "GET", ApiBaseUrl + "user", "");

            StringAssert.Matches(exception.InnerException?.Message, new Regex(errorMessage.TrimEnd('s') + "\\d\\.\\d+s"));
            StringAssert.Contains(exception.InnerException?.InnerException?.Message, "A task was canceled.");
            Assert.AreEqual(TimeSpan.FromSeconds(1), httpClient.Timeout);
        }
    }
}
