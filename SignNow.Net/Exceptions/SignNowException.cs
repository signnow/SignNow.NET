using System;
using System.Net;

namespace SignNow.Net.Exceptions
{
    public class SignNowException : Exception
    {
        private HttpStatusCode httpStatusCode;

        public HttpStatusCode HttpStatusCode
        {
            get
            {
                return this.httpStatusCode;
            }

            set
            {
                Data["HttpStatusCode"] = value;
                this.httpStatusCode = value;
            }
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
        /// with a specified error message and response status code.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        public SignNowException(string message, HttpStatusCode httpStatusCode) : base(message)
        {
            this.HttpStatusCode = httpStatusCode;
        }
    }
}
