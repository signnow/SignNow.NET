
using Newtonsoft.Json;
using SignNow.Net.Interface;
using SignNow.Net.Model;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net._Internal.Service
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

            return await ProcessResponse<TResponse>(response);
        }


        /// <summary>
        /// Creates Http Request from <see cref="SignNow.Net.Model.RequestOptions"/> class.
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <returns>Request Message <see cref="System.Net.Http.HttpRequestMessage"/></returns>
        private HttpRequestMessage CreateHttpRequest(RequestOptions requestOptions)
        {
            var httpMethod = new HttpMethod(requestOptions.HttpMethod);
            var requestMessage = new HttpRequestMessage(httpMethod, requestOptions.RequestUrl.ToString());


            requestMessage.Headers.Add("Authorization", requestOptions.Token.GetAccessToken());

            requestMessage.Content = this.CreateJsonContent(httpMethod, requestOptions.Content.ToString());

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
            if (method == HttpMethod.Post || method == HttpMethod.Put)
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
