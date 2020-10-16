using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Service;
using SignNow.Net.Model;
using SignNow.Net.Test;

namespace UnitTests
{
    [TestClass]
    public class SignNowClientTest : SignNowTestBase
    {
        [DataTestMethod]
        [DynamicData(nameof(ErrorContentProvider), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(TestDisplayName))]
        public void ShouldCatchAndProcessAnyOfErrorResponse(string testName, string errorContext, string expectedMessage)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

#pragma warning disable // CA2000 Dispose objects before losing scope
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(errorContext)
                })
                .Verifiable();

            // use real http client with mocked handler here
            var signnowClient = new SignNowClient(new HttpClient(handlerMock.Object, false));
#pragma warning restore // CA2000 Dispose objects before losing scope

            var exception = Assert
                .ThrowsException<AggregateException>(
                    () => signnowClient.RequestAsync(
                        new PostHttpRequestOptions { RequestUrl = ApiBaseUrl },
                        new HttpContentToObjectAdapter<Token>(new HttpContentToStringAdapter())).Result);

            Assert.AreEqual(expectedMessage, exception.InnerException?.Message);
        }

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
    }
}
