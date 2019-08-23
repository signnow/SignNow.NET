using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Internal.Requests
{
    internal class JsonHttpContent : IContent
    {
        private object contentBody;

        public JsonHttpContent(object contentBody)
        {
            this.contentBody = contentBody;
        }

        /// <summary>
        /// Creates Json Http Content from object
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(
                JsonConvert.SerializeObject(this.contentBody),
                Encoding.UTF8, "application/json");
        }
    }
}
