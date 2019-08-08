
using SignNow.Net.Model;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interface
{
    public interface ISignNowClient
    {
        Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default(CancellationToken));
    }
}
