using Newtonsoft.Json;
using SignNow.Net.Internal.Interfaces;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SignNow.Net.Internal.Helpers
{
    class HttpContentToObjectAdapter<TObject> : IHttpContentAdapter<TObject>
    {
        readonly IHttpContentAdapter<string> contentToStringAdapter;

        public HttpContentToObjectAdapter(IHttpContentAdapter<string> contentToStringAdapter)
        {
            this.contentToStringAdapter = contentToStringAdapter;
        }

        public async Task<TObject> Adapt(HttpContent content)
        {
            var stringContent = await contentToStringAdapter.Adapt(content).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TObject>(stringContent);
        }
    }

    class HttpContentToStringAdapter : IHttpContentAdapter<string>
    {
        public async Task<string> Adapt(HttpContent content)
        {
            return await content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }

    class HttpContentToStreamAdapter : IHttpContentAdapter<Stream>
    {
        public async Task<Stream> Adapt(HttpContent content)
        {
            return await content.ReadAsStreamAsync().ConfigureAwait(false);
        }
    }
}
