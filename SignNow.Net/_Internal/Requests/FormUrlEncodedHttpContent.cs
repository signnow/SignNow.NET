using System;
using System.Collections.Generic;
using System.Collections;
using System.Net.Http;
using System.Text;
using SignNow.Net.Interfaces;
using System.Net;
using System.Linq;

namespace SignNow.Net._Internal.Requests
{
    /// <summary>
    /// A container for name/value pairs encoded with <c>application/x-www-form-urlencoded</c>
    /// </summary>
    public class FormUrlEncodedHttpContent : ByteArrayContent, IContent
    {
        public FormUrlEncodedHttpContent(IDictionary<string, string> nameValueCollection)
            : base(CreateByteArrayContent(nameValueCollection))
        {
        }

        public HttpContent GetHttpContent()
        {
            return this;
        }

        private static byte[] CreateByteArrayContent(IDictionary<string, string> nameValueCollection)
        {
            var query = string.Join(
                "&",
                nameValueCollection.Select(
                    kvp => $"{UrlEncode(kvp.Key)}={UrlEncode(kvp.Value)}"));

            return Encoding.UTF8.GetBytes(query);
        }

        /// <summary>URL-encodes a string.</summary>
        /// <param name="value">The string to URL-encode.</param>
        /// <returns>The URL-encoded string.</returns>
        private static string UrlEncode(string value)
        {
            return WebUtility.UrlEncode(value)
                /* Don't use strict form encoding by changing the square bracket control
                 * characters back to their literals. This is fine by the server, and
                 * makes these parameter strings easier to read. */
                .Replace("%5B", "[")
                .Replace("%5D", "]");
        }
    }
}
