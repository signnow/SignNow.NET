using System.IO;
using System.Net.Http;
using SignNow.Net.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SignNow.Net.Internal.Requests
{
    /// <summary>
    /// A container for content encoded using <c>multipart/form-data</c> MIME type.
    /// </summary>
    internal class FileHttpContent : IContent
    {
        readonly Stream streamContent;
        readonly string fileName;

        public FileHttpContent(Stream content, string fileName)
        {
            this.streamContent = content;
            this.fileName = fileName;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose HttpContent object before losing scope", Justification = "It should be disposed via HttpContent.Dispose()")]
        public HttpContent GetHttpContent()
        {
            var content = new MultipartFormDataContent($"----{Guid.NewGuid():N}-----")
            {
                { new StreamContent(streamContent), "file", fileName }
            };

            return content;
        }
    }
}
