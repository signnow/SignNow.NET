using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;
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
        /// <returns>Response JSON content deserialized to <typeparamref name="TObject"/></returns>
        public async Task<TObject> Adapt(HttpContent content)
        {
            var stringContent = await contentToStringAdapter.Adapt(content).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<TObject>(stringContent);
        }
    }

    class HttpContentToStringAdapter : IHttpContentAdapter<string>
    {
        /// <inheritdoc />
        /// <returns>Content as a <see cref="string"/></returns>
        public async Task<string> Adapt(HttpContent content)
        {
            return await content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }

    class HttpContentToDownloadDocumentResponseAdapter : IHttpContentAdapter<DownloadDocumentResponse>
    {
        public async Task<DownloadDocumentResponse> Adapt(HttpContent content)
        {
            var rawStream = await content.ReadAsStreamAsync().ConfigureAwait(false);

            var document = new DownloadDocumentResponse
            {
                Filename = content.Headers.ContentDisposition?.FileName?.Replace("\"", ""),
                Length = content.Headers.ContentLength ?? default,
                Document = rawStream
            };

            return document;
        }
    }

    class HttpContentToStreamAdapter : IHttpContentAdapter<Stream>
    {
        /// <inheritdoc />
        /// <returns>Content as a <see cref="Stream"/></returns>
        public async Task<Stream> Adapt(HttpContent content)
        {
            return await content.ReadAsStreamAsync().ConfigureAwait(false);
        }
    }
}
