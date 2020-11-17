using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Model;
using SignNow.Net.Model;
using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;

namespace SignNow.Net.Internal.Service
{
    class SignNowClient : ISignNowClient
    {
        private static string sdkUserAgentString = string.Empty;

        private static string xUserAgentString = string.Empty;

        /// <summary>
        /// client_name>/version (OS_type OS_release; platform; arch) runtime/version
        /// </summary>
        public static string SdkUserAgentString
        {
            get
            {
                if (string.IsNullOrEmpty(sdkUserAgentString))
                    sdkUserAgentString = UserAgentSdkHeaders.BuildUserAgentString();

                return sdkUserAgentString;
            }
        }

        /// <summary>
        /// Platform dependent raw os/runtime string
        /// </summary>
        public static string XUserAgentString
        {
            get
            {
                if (string.IsNullOrEmpty(xUserAgentString))
                    xUserAgentString = UserAgentSdkHeaders.RawOsDescription();

                return xUserAgentString;
            }
        }

        private HttpClient HttpClient { get; }

        /// <summary>
        /// Initialize a new instance of SignNow Client
        /// </summary>
        /// <param name="httpClient">
        /// If <c>null</c>, an HTTP client will be created with default parameters.
        /// </param>
        public SignNowClient(HttpClient httpClient = null)
        {
#if NET45
            // With .NET Framework 4.5, it's necessary to manually enable support for TLS 1.2.
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#endif
            this.HttpClient = httpClient ?? new HttpClient();
            this.HttpClient.Timeout = TimeSpan.FromSeconds(180);
        }

        /// <inheritdoc cref="ISignNowClient.RequestAsync{TResponse}(RequestOptions, CancellationToken)" />
        public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default)
        {
            return await RequestAsync(
                requestOptions,
                new HttpContentToObjectAdapter<TResponse>(new HttpContentToStringAdapter()),
                HttpCompletionOption.ResponseContentRead,
                cancellationToken
                ).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignNowClient.RequestAsync{Stream}(RequestOptions, CancellationToken)" />
        public async Task<Stream> RequestAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default)
        {
            return await RequestAsync(
                requestOptions,
                new HttpContentToStreamAdapter(),
                HttpCompletionOption.ResponseContentRead,
                cancellationToken
                ).ConfigureAwait(false);
        }

        /// <inheritdoc cref="ISignNowClient.RequestAsync{TResponse}(RequestOptions, IHttpContentAdapter{TResponse}, HttpCompletionOption, CancellationToken)" />
        public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, IHttpContentAdapter<TResponse> adapter = default, HttpCompletionOption completionOption = default, CancellationToken cancellationToken = default)
        {
            Guard.ArgumentNotNull(adapter, nameof(adapter));

            DateTime startTime = DateTime.Now;

            using (var request = CreateHttpRequest(requestOptions))
            {
                try
                {
                    var response = await HttpClient
                        .SendAsync(request, completionOption, cancellationToken)
                        .ConfigureAwait(false);

                    await ProcessErrorResponse(requestOptions, response).ConfigureAwait(false);

                    return await adapter.Adapt(response.Content).ConfigureAwait(false);
                }
                catch (TaskCanceledException ex)
                {
                    var requestTime = (DateTime.Now - startTime).TotalSeconds;
                    var message = string.Format(CultureInfo.CurrentCulture,
                        ExceptionMessages.UnableToProcessRequest,
                        requestOptions.HttpMethod.Method,
                        requestOptions.RequestUrl.OriginalString,
                        requestTime);

                    throw new SignNowException(message, ex);
                }
            }
        }

        /// <summary>
        /// Process Error Response to prepare SignNow Exception
        /// </summary>
        /// <param name="requestOptions">request basic params (Url, Method)</param>
        /// <param name="response"><see cref="HttpResponseMessage"/></param>
        /// <exception cref="SignNowException">SignNow Exception.</exception>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Performance", "CA1825:Unnecessary zero-length array allocation", Justification = "Solution Array.Empty<>() works only for .NetStandard2.0, no significant memory or performance improvement")]
        private static async Task ProcessErrorResponse(RequestOptions requestOptions, HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage;

                var rawResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var snException = new SignNowException[0];

                try
                {
                    var converter = new HttpContentToObjectAdapter<ErrorResponse>(new HttpContentToStringAdapter());
                    var errorResponse = await converter.Adapt(response.Content).ConfigureAwait(false);

                    if (errorResponse.Errors?.Count > 1)
                    {
                        snException = errorResponse.Errors.Select(e => new SignNowException(e.Message)).ToArray();
                    }

                    errorMessage = errorResponse.GetErrorMessage();
                }
                // Catch error if something happened with Json body (possible broken response..)
                catch (Exception ex) when (ex is JsonSerializationException || ex is JsonReaderException)
                {
                    errorMessage = $"{ex.GetType()} thrown while parsing Json body from {requestOptions.RequestUrl.OriginalString}";
                    snException = new[] { new SignNowException(string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidJsonSyntax), ex) };
                }

                throw new SignNowException(errorMessage, snException)
                {
                    RawHeaders = response.Headers.ToDictionary(a => a.Key, a => a.Value),
                    RawResponse = rawResponse,
                    HttpStatusCode = response.StatusCode
                };
            }
        }

        /// <summary>
        /// Creates Http Request from <see cref="SignNow.Net.Model.RequestOptions"/> class.
        /// </summary>
        /// <param name="requestOptions"><see cref="RequestOptions"/></param>
        /// <exception cref="ArgumentException">The <paramref name="requestOptions">RequestUrl</paramref> argument is a null.</exception>
        /// <returns>Request Message <see cref="System.Net.Http.HttpRequestMessage"/></returns>
        private static HttpRequestMessage CreateHttpRequest(RequestOptions requestOptions)
        {
            Guard.PropertyNotNull(requestOptions.RequestUrl, ExceptionMessages.RequestUrlIsNull);

            var requestMessage = new HttpRequestMessage(requestOptions.HttpMethod, requestOptions.RequestUrl.ToString());

            requestMessage.Headers.Add("User-Agent", SdkUserAgentString);
            requestMessage.Headers.Add("X-User-Agent", XUserAgentString);

            if (null != requestOptions.Token)
            {
                requestMessage.Headers.Add("Authorization", requestOptions.Token.GetAuthorizationHeaderValue());
            }

            requestMessage.Content = requestOptions.Content?.GetHttpContent();

            return requestMessage;
        }
    }
}
