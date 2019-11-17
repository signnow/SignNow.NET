using System.Net.Http;
using System.Threading.Tasks;

namespace SignNow.Net.Interfaces
{
    public interface IHttpContentAdapter<TResult>
    {
        /// <summary>
        /// Convert Http content to corresponding Model.
        /// </summary>
        /// <param name="content">Http response content</param>
        /// <returns></returns>
        Task<TResult> Adapt(HttpContent content);
    }
}
