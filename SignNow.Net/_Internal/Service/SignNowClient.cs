
using Newtonsoft.Json;
using SignNow.Net.Interface;
using SignNow.Net.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net._Internal.Service
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
            client.DefaultRequestHeaders.Add("Content-Type", requestOptions.ContentType);

            var body = new Dictionary<string, string>
            {
               { "grant_type",requestOptions.GrantType },
               { "code", requestOptions.AuthorizationCode }               
            };

            //scope processing
            if (requestOptions.Scope == Scope.All)
                body.Add("scope", "*"); 
            else
                body.Add("scope", $"/{Scope.User.ToString().ToLower()}");

            var content = new FormUrlEncodedContent(body);
            var response = await client.PostAsync(requestOptions.URL, content);
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

