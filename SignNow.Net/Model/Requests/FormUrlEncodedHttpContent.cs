using System.Collections.Generic;
using System.Net.Http;
using SignNow.Net.Interfaces;

namespace SignNow.Net.Model.Requests
{
    /// <summary>
    /// A container for name/value pairs encoded with <c>application/x-www-form-urlencoded</c>
    /// </summary>
    public class FormUrlEncodedHttpContent : IContent
    {
        private readonly IDictionary<string, string> formData;

        public FormUrlEncodedHttpContent(IDictionary<string, string> nameValueCollection)
        {
            this.formData = nameValueCollection;
        }

        public HttpContent GetHttpContent()
        {
            return new FormUrlEncodedContent(this.formData);
        }
    }
}
