
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
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
            throw new NotImplementedException();
        }
    }
}
