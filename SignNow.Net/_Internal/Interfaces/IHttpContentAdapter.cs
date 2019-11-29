using System.Net.Http;
using System.Threading.Tasks;

namespace SignNow.Net.Internal.Interfaces
{
    interface IHttpContentAdapter<TResult>
    {
        Task<TResult> Adapt(HttpContent content);
    }
}
