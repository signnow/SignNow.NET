using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using System.Net;

namespace UnitTests
{
    [TestClass]
    public partial class SignNowExceptionTest
    {
        private const string TestMessage = "Test Exception Message";

        [TestMethod]
        public void ExceptionShouldHaveMessage()
        {
            var ex = new SignNowException(TestMessage);

            Assert.AreEqual(TestMessage, ex.Message);
        }

        [TestMethod]
        public void ExceptionShouldHaveMessageAndCode()
        {
            var ex = new SignNowException(TestMessage, HttpStatusCode.BadRequest);

            Assert.AreEqual(TestMessage, ex.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.HttpStatusCode);

            Assert.IsTrue(ex.Data.Contains("HttpStatusCode"));
            Assert.AreEqual((int) HttpStatusCode.BadRequest, ex.Data["HttpStatusCode"]);
        }

        [TestMethod]
        public void ExceptionStatusCodeCanBeOverriden()
        {
            var ex = new SignNowException(TestMessage, HttpStatusCode.Forbidden);

            Assert.AreEqual(HttpStatusCode.Forbidden, ex.HttpStatusCode);

            ex.HttpStatusCode = HttpStatusCode.InternalServerError;

            Assert.AreEqual(HttpStatusCode.InternalServerError, ex.HttpStatusCode);
        }

        [TestMethod]
        public void ExceptionShouldHaveInnerException()
        {
            var innerExMessage = "Test Forbidden Message";

            var innerEx = new SignNowException(innerExMessage, HttpStatusCode.Forbidden);
            var exception = new SignNowException(TestMessage, innerEx);
            var expectedMessage = TestMessage + $" ({innerExMessage})";

#if NET45
            expectedMessage = TestMessage;
#endif

            Assert.AreEqual(default, exception.HttpStatusCode);
            Assert.AreEqual(expectedMessage, exception.Message);
            Assert.AreEqual(HttpStatusCode.Forbidden, ((SignNowException) exception.InnerException).HttpStatusCode);
            Assert.AreEqual(innerExMessage, exception.InnerException.Message);
        }

        [TestMethod]
        public void ShouldBeSerializable()
        {
            var asJson = @"{
                'ClassName': 'SignNow.Net.Exceptions.SignNowException',
                'Message': 'One or more errors occurred.',
                'Data': null,
                'InnerException': null,
                'HelpURL': null,
                'StackTraceString': null,
                'RemoteStackTraceString': null,
                'RemoteStackIndex': 0,
                'ExceptionMethod': null,
                'HResult': -2146233088,
                'Source': null,
                'WatsonBuckets': null,
                'InnerExceptions': []
            }";

            var snExFromJson = TestUtils.DeserializeFromJson<SignNowException>(asJson);
            var serialized = TestUtils.SerializeToJsonFormatted(new SignNowException());

            StringAssert.Contains(serialized, "One or more errors occurred.");
            StringAssert.Contains(serialized, "InnerExceptions");
            StringAssert.Contains(serialized, "Message");
            StringAssert.Contains(serialized, "Data");

            Assert.That.JsonEqual(snExFromJson, serialized);
        }
    }
}
