using System.IO;
using System.Net.Http;
using SignNow.Net.Interfaces;
using System;
using System.Text;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// A container for content encoded using <c>multipart/form-data</c> MIME type.
    /// </summary>
    internal class MultipartHttpContent : IContent
    {
        private readonly Stream _streamContent;
        private readonly string _fileName;
        private readonly ITextTags _textTags;

        public MultipartHttpContent(Stream content, string fileName, ITextTags tags)
        {
            _streamContent = content;
            _fileName = fileName;
            _textTags = tags;
        }

        // [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose HttpContent object before losing scope", Justification = "It should be disposed via HttpContent.Dispose()")]
        public HttpContent GetHttpContent()
        {
            var content = new MultipartFormDataContent($"----{Guid.NewGuid():N}-----")
            {
                { new StreamContent(_streamContent), "file", _fileName }
            };

            if (_textTags != null)
            {
                content.Add(new StringContent(_textTags.ToStringContent(), Encoding.UTF8), "Tags");
            }

            return content;
        }
    }
}
