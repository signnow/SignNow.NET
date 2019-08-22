
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Internal.Service
{
    class SignNowClient : ISignNowClient
    {
        private System.Net.Http.HttpClient HttpClient { get; }

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
            var request = CreateHttpRequest(requestOptions);

            var response = await this.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            return await HandleResponse<TResponse>(response);
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
                requestMessage.Headers.Add("Authorization", requestOptions.Token.GetAccessToken());
            }

            if (requestOptions.HttpMethod == HttpMethod.Post || requestOptions.HttpMethod == HttpMethod.Put)
            {
                requestMessage.Content = requestOptions.Content?.GetHttpContent();
            }

            return requestMessage;
        }

        /// <summary>
        /// Process raw HTTP response into requested domain type.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="response">The <see cref="HttpResponseMessage"/> to handle</param>
        /// <returns></returns>
        private async Task<TResponse> HandleResponse<TResponse>(HttpResponseMessage response)
        {
            var mimeType = response.Content.Headers.ContentType?.MediaType;
            var responseObj = default(TResponse);

            switch (mimeType)
            {
                case "application/json":
                    var contentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    responseObj = JsonConvert.DeserializeObject<TResponse>(contentAsString);
                    break;

                case "application/octet-stream":
                    var streamContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    string content;

                    using (StreamReader streamReader = new StreamReader(streamContent, System.Text.Encoding.Unicode))
                    {
                        content = streamReader.ReadToEnd();
                    }

                    responseObj = JsonConvert.DeserializeObject<TResponse>(content);
                    break;

                default:
                    break;
            }


            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new SignNowException(response.ReasonPhrase, ex);
            }

            return responseObj;
        }
    }
}
