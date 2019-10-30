using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization;

namespace SignNow.Net.Exceptions
{
#if NET45 || NETSTANDARD2_0
    [Serializable]
#endif
    public class SignNowException : AggregateException
    {
        private HttpStatusCode httpStatusCode;

        public HttpStatusCode HttpStatusCode
        {
            get { return httpStatusCode; }

            set
            {
                Data["HttpStatusCode"] = (int)value;
                httpStatusCode = value;
            }
        }

        public HttpResponseHeaders RawHeaders
        {
            get
            {
                return Data.Contains("RawHeaders") ? (HttpResponseHeaders)Data["RawHeaders"] : default;
            }

            set { Data["RawHeaders"] = value; }
        }

        public string RawResponse
        {
            get
            {
                return Data.Contains("RawResponse") ? (string)Data["RawResponse"] : String.Empty;
            }

            set { Data["RawResponse"] = value;  }
        }

        public SignNowException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignNow.Net.Exceptions.SignNowException" /> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        public SignNowException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignNow.Net.Exceptions.SignNowException" /> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified. </param>
        public SignNowException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignNow.Net.Exceptions.SignNowException" /> class
        /// with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        public SignNowException(string message, IEnumerable<SignNowException> innerExceptions) : base(message, innerExceptions) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignNow.Net.Exceptions.SignNowException" /> class
        /// with a specified error message and response status code.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="httpStatusCode">Http status code related to exception</param>
        public SignNowException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

    #if NET45 || NETSTANDARD2_0
        protected SignNowException(SerializationInfo info, StreamingContext context) : base (info, context) { }
    #endif
    }
}
