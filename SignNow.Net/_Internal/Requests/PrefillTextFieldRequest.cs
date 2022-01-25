using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SignNow.Net.Interfaces;
using SignNow.Net.Model;

namespace SignNow.Net.Internal.Requests
{
    internal class PrefillTextFieldRequest : IContent
    {
        /// <summary>
        /// Collections of <see cref="PrefillTextField"/> request options.
        /// </summary>
        [JsonProperty("fields")]
        public List<PrefillTextField> Fields { get; set; } = new List<PrefillTextField>();

        public PrefillTextFieldRequest() {}

        public PrefillTextFieldRequest(IEnumerable<PrefillTextField> fields)
        {
            Fields = fields.ToList();
        }

        /// <summary>
        /// Creates Json Http Content from object
        /// </summary>
        /// <returns>HttpContent</returns>
        public HttpContent GetHttpContent()
        {
            return new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
        }
    }
}
