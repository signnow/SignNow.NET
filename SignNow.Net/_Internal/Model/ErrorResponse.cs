using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;

namespace SignNow.Net.Internal.Model
{
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "The class is used for JSON deserialization")]
    class ErrorResponse
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
        [SuppressMessage("Microsoft.Usage", "CA2227:Change Errors to be readonly", Justification = "Json deserialization will not work without set")]
        public List<ErrorResponseContext> Errors { get; set; }

        public string GetErrorMessage()
        {
            var message = string.Empty;

            if (!string.IsNullOrEmpty(Error))
                message = Error;

            if (!string.IsNullOrEmpty(Error404))
                message = Error404;

            if (Errors != null)
            {
                if (Errors.Count == 1)
                    message = Errors[0].Message;

                // Aggergate all Messages
                if (Errors.Count > 1)
                {
                    var strBuilder = new StringBuilder();

                    foreach (ErrorResponseContext item in Errors)
                    {
                        strBuilder.AppendLine(item.Message);
                    }
                    message = strBuilder.ToString();
                }
            }
           
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
