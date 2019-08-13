using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignNow.Net.Interface;
using SignNow.Net.Model;

namespace SignNow.Net
{
    class SignNowClient : HttpClient, ISignNowClient
    {
        private System.Net.Http.HttpClient HttpClient { get; }

        /// <summary>
        /// Initialize a new instance of SignNow Client
        /// </summary>
        /// <param name="httpClient">
        /// The <see cref="ISignNowClient"/> client to use. If <c>null</c>, an HTTP client will be
        /// created with default parameters.
        /// </param>
        public SignNowClient(HttpClient httpClient = null)
        {
            this.HttpClient = httpClient ?? MakeDefaultHttpClient();
        }

        public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken)
        {
            var request = CreateHttpRequest(requestOptions);

            var response = await this.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            return ProcessResponse<TResponse>(response);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="System.Net.Http.HttpClient"/> class
        /// with default parameters.
        /// </summary>
        /// <returns>The new instance of the <see cref="System.Net.Http.HttpClient"/> class.</returns>
        private HttpClient MakeDefaultHttpClient()
        {
            return new HttpClient();
        }

        private HttpRequestMessage CreateHttpRequest(RequestOptions requestOptions)
        {
            var httpMethod = new HttpMethod(requestOptions.Method);
            var requestMessage = new HttpRequestMessage(httpMethod, requestOptions.Uri);


            foreach (var header in requestOptions.Headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }

            requestMessage.Content = this.CreateContent(httpMethod, requestOptions.Content);

            return requestMessage;
        }

        private HttpContent CreateContent(HttpMethod method, string content)
        {
            if (method == HttpMethod.Post)
            {
                return new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            }

            return null;
        }

        private TResponse ProcessResponse<TResponse>(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new NotImplementedException();
            }

            TResponse responseObj;

            var content = response.Content.ReadAsStringAsync().Result;
            responseObj = JsonConvert.SerializeObject(content);

            return responseObj;
        }
    }
}
