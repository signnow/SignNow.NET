using System.Net.Http;
using System.Text;
using SignNow.Net.Interfaces;

namespace SignNow.Net._Internal.Requests
{
    public class JsonHttpContent : IContent
    {
        private string ContentBody;

        public JsonHttpContent(string contentBody)
        {
            this.ContentBody = contentBody;
        }

        /// <summary>
        /// Creates Json Http Content from string
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent Content()
        {
            return new StringContent(this.ContentBody, Encoding.UTF8, "application/json");
        }
    }
}
