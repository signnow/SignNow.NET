using System.IO;
using System.Net.Http;
using SignNow.Net.Interfaces;
using System;

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

        public HttpContent GetHttpContent()
        {
            //return CreateStreamContent(this.streamContent);
            var content = new MultipartFormDataContent($"----{Guid.NewGuid().ToString("N")}-----");
#pragma warning disable CA2000 // Dispose objects before losing scope. It should be disposed via HttpContent.Dispose()
            content.Add(new StreamContent(streamContent), "file", fileName);
#pragma warning restore CA2000 // Dispose objects before losing scope
            return content;
        }
    }
}
