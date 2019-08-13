using System.Collections.Generic;

namespace SignNow.Net.Model
{
    public class RequestOptions
    {
        public string Method { get; set; }

        public string Uri { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Content { get; set; } = string.Empty;
    }
}