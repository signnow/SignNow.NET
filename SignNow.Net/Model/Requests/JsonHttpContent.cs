using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.Requests
{
    public abstract class JsonHttpContent : IContent
    {
        // private readonly object contentBody;

        // public JsonHttpContent(object contentBody)
        // {
        //     this.contentBody = contentBody;
        // }

        /// <summary>
        /// Creates Json Http Content from object
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(
                JsonConvert.SerializeObject(this),
                Encoding.UTF8, "application/json");
        }
    }
}
