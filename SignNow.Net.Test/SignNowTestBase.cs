using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Service;

namespace SignNow.Net.Test
{
    public abstract class SignNowTestBase
    {
        public static Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");
        public static Uri ApplicationBaseUrl => new Uri("https://app-eval.signnow.com/");

        private static readonly string BaseTestDataPath = "../../../TestData/"
            .Replace('/', Path.DirectorySeparatorChar);

#pragma warning disable CA1051 // Do not declare visible instance fields [SignNow.Net.Test]csharp(CA1051)
        protected static readonly string PdfFileName = "DocumentUpload.pdf";
        protected static readonly string TxtFileName = "DocumentUpload.txt";
#pragma warning restore CA1051 // Do not declare visible instance fields

        protected static string PdfFilePath => Path.Combine($"{BaseTestDataPath}Documents", PdfFileName);
        protected static string TxtFilePath => Path.Combine($"{BaseTestDataPath}Documents", TxtFileName);

        /// <summary>
        /// Create Mock for HttpClient
        /// </summary>
        /// <param name="jsonResponse">Json response that you want to be returned</param>
        /// <param name="code">Http status that you expect</param>
        /// <returns></returns>
        protected ISignNowClient SignNowClientMock(string jsonResponse, HttpStatusCode code = HttpStatusCode.OK)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = code,
                    Content = new StringContent(jsonResponse)
                })
                .Verifiable();

            // use real http client with mocked handler here
            return new SignNowClient(new HttpClient(handlerMock.Object, false));
        }
    }
}
