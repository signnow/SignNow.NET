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
        /// If <c>null</c>, an HTTP client will be created with default parameters.
        /// </param>
        public SignNowClient(HttpClient httpClient = null)
        {
            this.HttpClient = httpClient ?? MakeDefaultHttpClient();
        }

        public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken)
        {
            var request = CreateHttpRequest(requestOptions);

            var response = await this.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            return await ProcessResponse<TResponse>(response);
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

        /// <summary>
        /// Creates Http Request from <see cref="SignNow.Net.Model.RequestOptions"/> class.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns>Request Message <see cref="System.Net.Http.HttpRequestMessage"/></returns>
        private HttpRequestMessage CreateHttpRequest(RequestOptions requestOptions)
        {
            var httpMethod = new HttpMethod(requestOptions.Method);
            var requestMessage = new HttpRequestMessage(httpMethod, requestOptions.Uri);


            foreach (var header in requestOptions.Headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }

            requestMessage.Content = this.CreateJsonContent(httpMethod, requestOptions.Content);

            return requestMessage;
        }

        /// <summary>
        /// Prepare Json Content from String
        /// </summary>
        /// <param name="method">Request Method</param>
        /// <param name="content">String content</param>
        /// <returns></returns>
        private HttpContent CreateJsonContent(HttpMethod method, string content)
        {
            if (method == HttpMethod.Post)
            {
                return new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            }

            return null;
        }

        private async Task<TResponse> ProcessResponse<TResponse>(HttpResponseMessage response)
        {
            var contentAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                throw new NotImplementedException();
            }

            return JsonConvert.DeserializeObject<TResponse>(contentAsString);
        }
    }
}
