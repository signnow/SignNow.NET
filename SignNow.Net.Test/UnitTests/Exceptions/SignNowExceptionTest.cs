using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using System;
using System.Net;

namespace UnitTests
{
    [TestClass]
    public partial class SignNowExceptionTest
    {
        private readonly static string testMessage = "Test Exception Message";

        private readonly string assertExceptionPropertyShouldHaveValue = "Exception '{0}' should have '{1}'";
        private readonly string assertExceptionPropertyShouldBe        = "Exception '{0}' should be '{1}'";

        [TestMethod]
        public void Exception_Has_Message()
        {
            var ex = new SignNowException(testMessage);

            Assert.AreEqual(
                testMessage,
                ex.Message,
                String.Format(assertExceptionPropertyShouldHaveValue, "Message", testMessage)
                );
        }

        [TestMethod]
        public void Exception_Has_Message_And_Code()
        {
            var ex = new SignNowException(testMessage, HttpStatusCode.BadRequest);

            Assert.AreEqual(
                testMessage,
                ex.Message,
                String.Format(assertExceptionPropertyShouldHaveValue, ex.Message.GetType().ToString(), testMessage)
                );

            Assert.AreEqual(
                HttpStatusCode.BadRequest,
                ex.HttpStatusCode,
                String.Format(assertExceptionPropertyShouldBe, "HttpStatusCode", HttpStatusCode.BadRequest.ToString())
                );

            Assert.IsTrue(ex.Data.Contains("HttpStatusCode"));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ex.Data["HttpStatusCode"]);
        }

        [TestMethod]
        public void Exception_StatusCode_Can_Be_Overriden()
        {
            var ex = new SignNowException(testMessage, HttpStatusCode.Forbidden);

            Assert.AreEqual(
                HttpStatusCode.Forbidden,
                ex.HttpStatusCode,
                String.Format(assertExceptionPropertyShouldBe, "HttpStatusCode", HttpStatusCode.Forbidden.ToString())
                );

            ex.HttpStatusCode = HttpStatusCode.InternalServerError;

            Assert.AreEqual(
                HttpStatusCode.InternalServerError,
                ex.HttpStatusCode,
                String.Format(assertExceptionPropertyShouldBe, "HttpStatusCode", HttpStatusCode.InternalServerError.ToString())
                );
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