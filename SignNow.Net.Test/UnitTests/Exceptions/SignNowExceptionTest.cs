using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Newtonsoft.Json;

namespace UnitTests
{
    [SuppressMessage("Microsoft.Globalization", "CA1303:Message string literals should be taken from resource file",
        Justification = "In the unit tests below no reason to use resource file")]
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

            var snExFromJson = JsonConvert.DeserializeObject<SignNowException>(asJson);
            var serialized = JsonConvert.SerializeObject(new SignNowException());

            StringAssert.Contains(serialized, "One or more errors occurred.");
            StringAssert.Contains(serialized, "InnerExceptions");
            StringAssert.Contains(serialized, "Message");
            StringAssert.Contains(serialized, "Data");

            AssertJson.AreEqual(snExFromJson, serialized);
        }
    }
}
