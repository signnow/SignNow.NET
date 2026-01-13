using System.Net.Http;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// Empty payload request for endpoints that don't require request body
    /// </summary>
    public class EmptyPayloadRequest : IContent
    {
        /// <summary>
        /// Gets the HTTP content for the empty payload request
        /// </summary>
        /// <returns>HTTP content representing an empty JSON object</returns>
        public HttpContent GetHttpContent()
        {
            var json = JsonConvert.SerializeObject(new { });
            return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        }
    }
}