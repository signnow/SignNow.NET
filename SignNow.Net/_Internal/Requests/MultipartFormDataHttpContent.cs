using System.IO;
using System.Net.Http;
using SignNow.Net.Interfaces;
using System.Net.Http.Headers;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// A container for content encoded using <c>multipart/form-data</c> MIME type.
    /// </summary>
    internal class MultipartFormDataHttpContent : IContent
    {
        private Stream streamContent;

        public MultipartFormDataHttpContent(Stream content)
        {
            this.streamContent = content;
        }

        public HttpContent GetHttpContent()
        {
            return CreateStreamContent(this.streamContent);
        }

        private StreamContent CreateStreamContent(Stream content)
        {
            var _streamContent = new StreamContent(content);

            _streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file"
            };

            _streamContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

            return _streamContent;
        }
    }
}
