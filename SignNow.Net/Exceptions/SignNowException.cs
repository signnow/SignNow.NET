using System;

namespace SignNow.Net.Exceptions
{
    public class SignNowException : Exception
    {
        public SignNowException()
        {
        }

        public SignNowException(string message) : base(message)
        {
        }

        public SignNowException(string message, Exception previous) : base(message, previous)
        {
        }
    }
}
