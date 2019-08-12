using System;
using System.Threading;
using System.Threading.Tasks;
using SignNow.Net.Interface;
using SignNow.Net.Model;

namespace SignNow.Net
{
    public class SignNowClient : ISignNowClient
    {
        public SignNowClient()
        {
        }

        Task<TResponse> ISignNowClient.RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
