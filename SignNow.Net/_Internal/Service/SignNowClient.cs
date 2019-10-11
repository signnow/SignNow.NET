
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Internal.Interfaces;
using SignNow.Net.Internal.Model;
using SignNow.Net.Model;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Internal.Service
{
    class SignNowClient : ISignNowClient
    {
        private HttpClient HttpClient { get; }

        /// <summary>
        /// Initialize a new instance of SignNow Client
        /// </summary>
        /// <param name="httpClient">
        /// If <c>null</c>, an HTTP client will be created with default parameters.
        /// </param>
        public SignNowClient(HttpClient httpClient = null)
        {
            this.HttpClient = httpClient ?? new HttpClient();
        }

        /// <summary>
        /// HTTP requests are being made here
        /// </summary>
        /// <typeparam name="TResponse">Type (Model) of the response from the request</typeparam>
        /// <param name="requestOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default)
        {
            return await RequestAsync(requestOptions, new HttpContentToObjectAdapter<TResponse>(new HttpContentToStringAdapter()), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// HTTP requests which returns Stream response
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RequestAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default)
        {
            await RequestAsync(requestOptions, new HttpContentToStreamAdapter(), cancellationToken).ConfigureAwait(false);
        }

        private async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, IHttpContentAdapter<TResponse> adapter, CancellationToken cancellationToken = default)
        {
            using (var request = CreateHttpRequest(requestOptions))
            using (var response = await this.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                await ProcessErrorResponse(response).ConfigureAwait(false);

                return await adapter.Adapt(response.Content).ConfigureAwait(false);
            }
        }

        private async Task ProcessErrorResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var context = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var apiError = context;

                try
                {
                    var converter = new HttpContentToObjectAdapter<ErrorResponse>(new HttpContentToStringAdapter());
                    var errorResponse = await converter.Adapt(response.Content).ConfigureAwait(false);

                    apiError = errorResponse.GetErrorMessage();

                }
                catch (JsonSerializationException)
                {
                }

                throw new SignNowException(apiError, response.StatusCode);
            }
        }

        /// <summary>
        /// Creates Http Request from <see cref="SignNow.Net.Model.RequestOptions"/> class.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns>Request Message <see cref="System.Net.Http.HttpRequestMessage"/></returns>
        private HttpRequestMessage CreateHttpRequest(RequestOptions requestOptions)
        {
            if (requestOptions.RequestUrl == null)
            {
                throw new ArgumentException("RequestUrl cannot be empty or null.");
            }

            var requestMessage = new HttpRequestMessage(requestOptions.HttpMethod, requestOptions.RequestUrl.ToString());

            if (requestOptions.Token != null)
            {
                requestMessage.Headers.Add("Authorization", requestOptions.Token.GetAuthorizationHeaderValue());
            }

            requestMessage.Content = requestOptions.Content?.GetHttpContent();

            return requestMessage;
        }
    }
}
