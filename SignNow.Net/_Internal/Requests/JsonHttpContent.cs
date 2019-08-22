using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net._Internal.Requests
{
    public class JsonHttpContent : IContent
    {
        private object ContentBody;

        public JsonHttpContent(object contentBody)
        {
            this.ContentBody = contentBody;
        }

        /// <summary>
        /// Creates Json Http Content from string
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(
                JsonConvert.SerializeObject(this.ContentBody),
                Encoding.UTF8, "application/json");
        }
    }
}
