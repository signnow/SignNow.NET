using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace UnitTests
{
    [SuppressMessage("Microsoft.Globalization", "CA1303:Message string literals should be taken from resource file", Justification = "In the unit tests below no reason to use resource file")]
    [TestClass]
    public partial class SignNowExceptionTest
    {
        private const string testMessage = "Test Exception Message";

        [TestMethod]
        public void Exception_Has_Message()
        {
            var ex = new SignNowException(testMessage);

            Assert.AreEqual(testMessage, ex.Message);
        }

        [TestMethod]
        public void Exception_Has_Message_And_Code()
        {
            var ex = new SignNowException(testMessage, HttpStatusCode.BadRequest);

            Assert.AreEqual(testMessage, ex.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.HttpStatusCode);

            Assert.IsTrue(ex.Data.Contains("HttpStatusCode"));
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ex.Data["HttpStatusCode"]);
        }

        [TestMethod]
        public void Exception_StatusCode_Can_Be_Overriden()
        {
            var ex = new SignNowException(testMessage, HttpStatusCode.Forbidden);

            Assert.AreEqual(HttpStatusCode.Forbidden, ex.HttpStatusCode);

            ex.HttpStatusCode = HttpStatusCode.InternalServerError;

            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.HttpStatusCode);
        }

        [TestMethod]
        public void Exception_Have_InnerException()
        {
            var innerExMessage = "Test Forbidden Message";

            var innerEx   = new SignNowException(innerExMessage, HttpStatusCode.Forbidden);
            var exception = new SignNowException(testMessage, innerEx);
            var expectedMessage = testMessage + $" ({innerExMessage})";

#if NET45
            expectedMessage = testMessage;
#endif

            Assert.AreEqual(default, exception.HttpStatusCode);
            Assert.AreEqual(expectedMessage, exception.Message);
            Assert.AreEqual(HttpStatusCode.Forbidden, ((SignNowException)exception.InnerException).HttpStatusCode);
            Assert.AreEqual(innerExMessage, exception.InnerException.Message);
        }
    }
}