using SignNow.Net.Model;
using System.IO;
using System.Net.Http;
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
        /// <param name="requestOptions"><see cref="RequestOptions"/></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TResponse"/></returns>
        Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISignNowClient.RequestAsync{TResponse}(RequestOptions, CancellationToken)"/>
        /// <typeparam name="TResponse">Type (Model) of the response from the request</typeparam>
        /// <param name="requestOptions"><see cref="RequestOptions"/></param>
        /// <param name="adapter"><see cref="IHttpContentAdapter{TResponse}"/></param>
        /// <param name="completionOption"><see cref="HttpCompletionOption"/></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="TResponse"/></returns>
        Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, IHttpContentAdapter<TResponse> adapter = default, HttpCompletionOption completionOption = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// HTTP requests which returns Stream response
        /// </summary>
        /// <param name="requestOptions"><see cref="RequestOptions"/></param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled</param>
        /// <returns><see cref="Stream"/> response.</returns>
        Task<Stream> RequestAsync(RequestOptions requestOptions, CancellationToken cancellationToken = default);
    }
}
