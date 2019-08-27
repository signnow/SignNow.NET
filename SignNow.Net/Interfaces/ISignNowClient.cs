
using SignNow.Net.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface ISignNowClient
    {
        Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default(CancellationToken));

        Task<Stream> RequestAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default(CancellationToken));
    }
}
