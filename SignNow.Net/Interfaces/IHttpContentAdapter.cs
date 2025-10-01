using System.Net.Http;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface IHttpContentAdapter<TResult>
    {
        /// <summary>
        /// Converts HTTP content to <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="content">Http response content</param>
        Task<TResult> Adapt(HttpContent content);
    }
}
