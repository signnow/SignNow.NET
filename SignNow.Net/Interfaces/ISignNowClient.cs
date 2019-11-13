using SignNow.Net.Model;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface ISignNowClient
    {
        /// <summary>
        /// HTTP requests are being made here
        /// </summary>
        /// <typeparam name="TResponse">Type (Model) of the response from the request</typeparam>
        /// <param name="requestOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default);

        /// <summary>
        /// HTTP requests which returns Stream response
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Stream> RequestAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default);

        /// <summary>
        /// HTTP requests which returns Resource response
        /// </summary>
        /// <param name="requestOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DownloadDocumentResponse> RequestResourceAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default);
    }
}
