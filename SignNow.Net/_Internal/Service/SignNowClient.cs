
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Internal.Service
{
    class SignNowClient : ISignNowClient
    {
        /// <summary>
        /// HTTP requests are being made here
        /// </summary>
        /// <typeparam name="TResponse">Type (Model) of the response from the request</typeparam>
        /// <param name="requestOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default)
        {
            var client = new HttpClient();

            var plainTextBytes = Encoding.UTF8.GetBytes($"{requestOptions.ClientId}:{requestOptions.ClientSecret}");
            var appToken = Convert.ToBase64String(plainTextBytes);

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {appToken}");
            client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue(requestOptions.ContentType));

            var body = new Dictionary<string, string>
            {
               { "grant_type",requestOptions.GrantType },
               { "username", requestOptions.Login },
               { "password", requestOptions.Password }
            };

            //scope processing
            if (requestOptions.Scope == Scope.All)
                body.Add("scope", "*");
            else
                body.Add("scope", $"/{Scope.User.ToString().ToLower()}");

            var content = new FormUrlEncodedContent(body);
            var response = await client.PostAsync("https://api-eval.signnow.com/oauth2/token", content);
            var responseString = await response.Content.ReadAsStringAsync();
            response.Dispose();

            if (cancellationToken.IsCancellationRequested)
            {
                return default;
            }

            var responce = JsonConvert.DeserializeObject<TResponse>(responseString);
            return responce;
        }
    }
}
