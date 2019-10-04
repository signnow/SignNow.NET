using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;

namespace UnitTests
{
    public partial class SignNowExceptionTest
    {
        [TestMethod]
        public void Exception_Has_Proper_Data_Context()
        {
            var innerException = new Exception("Inner Exception Message");
            var testException  = new SignNowException(testMessage, innerException);

            Assert.AreEqual(
                testMessage,
                testException.Message,
                String.Format(assertExceptionPropertyShouldHaveValue, "Message", testMessage)
                );

            Assert.AreEqual(
                "Inner Exception Message",
                testException.InnerException.Message,
                String.Format(assertExceptionPropertyShouldHaveValue, "Message", innerException.Message)
                );
        }
    }
}
