using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignNow.Net.Internal.Model
{
    public class ErrorResponse
    {
        /// <summary>
        /// Error Message in some response cases
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// Error Message for non-valid URL
        /// </summary>
        [JsonProperty("404")]
        public string Error404 { get; set; }

        [JsonProperty("errors")]
        public List<ErrorResponseContext> Errors { get; set; }

        public string GetErrorMessage()
        {
            var message = string.Empty;

            if (!string.IsNullOrEmpty(Error))
                message = Error;

            if (!string.IsNullOrEmpty(Error404))
                message = Error404;

            if (Errors != null && Errors.Count == 1)
                message = Errors[0].Message;

            return message;
        }
    }

    public class ErrorResponseContext
    {
        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public int Code { get; set; }
    }
}
