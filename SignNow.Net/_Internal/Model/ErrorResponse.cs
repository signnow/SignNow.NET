using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignNow.Net.Internal.Model
{
    internal class ErrorResponseContext
    {
        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public string Code { get; set; }
    }

    internal class ErrorResponse
    {
        /// <summary>
        /// Error Message in some response cases
        /// </summary>
        [JsonProperty("error")]
        public object Error { get; set; }

        /// <summary>
        /// Error code in some response cases
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Error Message for non-valid URL
        /// </summary>
        [JsonProperty("404")]
        public string Error404 { get; set; }

        [JsonProperty("errors")]
        public List<ErrorResponseContext> Errors { get; set; }

        public string GetErrorCode()
        {
            if (Error is JArray { Count: 1 } error)
            {
                return error[0].ToObject<ErrorResponseContext>()?.Code;
            }

            if (Errors is { Count: 1 }) { return Errors[0].Code; }

            if (Errors != null && Errors.Count > 1)
            {
                var strBuilder = new StringBuilder();
                foreach (ErrorResponseContext item in Errors)
                {
                    strBuilder.AppendLine(item.Code);
                }
                return strBuilder.ToString();
            }

            return Code ?? String.Empty;
        }

        public string GetErrorMessage()
        {
            switch (Error)
            {
                case string err:
                    return err;
                case JArray { Count: 1 } error:
                    return error[0].ToObject<ErrorResponseContext>()?.Message;
            }

            if (Errors is { Count: 1 }) { return Errors[0].Message; }


            if (Errors != null && Errors.Count > 1)
            {
                var strBuilder = new StringBuilder();
                foreach (ErrorResponseContext item in Errors)
                {
                    strBuilder.AppendLine(item.Message);
                }
                return strBuilder.ToString();
            }

            return Error404 ?? String.Empty;
        }
    }
}
