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

        /// <inheritdoc />
        /// <returns><see cref="TObject"/> Models</returns>
        public async Task<TObject> Adapt(HttpContent content)
        {
            var stringContent = await contentToStringAdapter.Adapt(content).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TObject>(stringContent);
        }
    }

    class HttpContentToStringAdapter : IHttpContentAdapter<string>
    {
        /// <inheritdoc />
        /// <returns><see cref="string"/> content</returns>
        public async Task<string> Adapt(HttpContent content)
        {
            return await content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }

    class HttpContentToStreamAdapter : IHttpContentAdapter<Stream>
    {
        /// <inheritdoc />
        /// <returns><see cref="Stream"/> content</returns>
        public async Task<Stream> Adapt(HttpContent content)
        {
            return await content.ReadAsStreamAsync().ConfigureAwait(false);
        }
    }
}
