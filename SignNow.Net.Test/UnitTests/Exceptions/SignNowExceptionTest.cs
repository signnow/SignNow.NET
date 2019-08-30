using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using System;
using System.Net;

namespace UnitTests
{
    [TestClass]
    public class SignNowExceptionTest
    {
        private readonly string testMessage = "Test Exception Message";

        [TestMethod]
        public void Exception_Has_Message()
        {
            var ex = new SignNowException(testMessage);

            Assert.AreEqual(testMessage, ex.Message, $"Exception Message should have '{testMessage}'");
        }

        [TestMethod]
        public void Exception_Has_Message_And_Code()
        {
            var ex = new SignNowException(testMessage, HttpStatusCode.BadRequest);

            Assert.AreEqual(testMessage, ex.Message, $"Exception {ex.Message.GetType().ToString()} should have '{testMessage}'");
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.HttpStatusCode, $"Exception HttpStatusCode should be '{HttpStatusCode.BadRequest.ToString()}'");
        }

        [TestMethod]
        public void Exception_StatusCode_Can_Be_Overriden()
        {
            var ex = new SignNowException(testMessage, HttpStatusCode.Forbidden);

            Assert.AreEqual(HttpStatusCode.Forbidden, ex.HttpStatusCode, $"Exception HttpStatusCode should be '{HttpStatusCode.Forbidden.ToString()}'");

            ex.HttpStatusCode = HttpStatusCode.InternalServerError;

            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.HttpStatusCode, $"Exception HttpStatusCode should be '{HttpStatusCode.InternalServerError.ToString()}'");
        }

        [TestMethod]
        public void Exception_Have_InnerException()
        {
            var innerEx = new SignNowException("Inner Exception Message", HttpStatusCode.Forbidden);
            var exception = new SignNowException(testMessage, innerEx);

            Assert.AreEqual(default, exception.HttpStatusCode);
            Assert.AreEqual(testMessage, exception.Message);
            Assert.AreEqual(HttpStatusCode.Forbidden, ((SignNowException)exception.InnerException).HttpStatusCode);
            Assert.AreEqual("Inner Exception Message", exception.InnerException.Message);
        }
    }
}