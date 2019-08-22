namespace SignNow.Net._Internal.Requests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using SignNow.Net.Interfaces;

    /// <summary>
    /// A container for name/value pairs encoded with <c>application/x-www-form-urlencoded</c>
    /// </summary>
    internal class FormUrlEncodedHttpContent : IContent
    {
        private IDictionary<string, string> formData;

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
