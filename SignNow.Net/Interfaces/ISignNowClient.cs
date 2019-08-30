
using SignNow.Net.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface ISignNowClient
    {
        Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default);

        Task RequestAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default);
    }
}
