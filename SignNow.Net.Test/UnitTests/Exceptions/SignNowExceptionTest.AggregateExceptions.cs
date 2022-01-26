using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;

namespace UnitTests
{
    public partial class SignNowExceptionTest
    {
        [TestMethod]
        public void ExceptionShouldHaveProperDataContext()
        {
            var testException = new SignNowException(TestMessage)
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Data =
                {
                    ["HttpStatusCode"] = (int)HttpStatusCode.BadRequest,
                    ["Request"] = "Test Request Body"
                }
            };

            Assert.AreEqual(TestMessage, testException.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, testException.HttpStatusCode);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, testException.Data["HttpStatusCode"]);
            Assert.AreEqual("Test Request Body", testException.Data["Request"]);
        }

        [TestMethod]
        public void ExceptionShouldContainsExceptionsList()
        {
            var snExceptions = new List<SignNowException>();

            var generalMessage = "L1 Ex Message Example";
            var snAggregateMsg = new StringBuilder(generalMessage);

            for (var i = 0; i < 10; i++)
            {
                snExceptions.Add(new SignNowException($"inner-exception {i}"));
                snAggregateMsg.AppendFormat(" (inner-exception {0})", i);
            }

            var snEx  = new SignNowException(generalMessage, snExceptions);
            var expectedMessage = snAggregateMsg.ToString();

            Assert.AreEqual(10, snEx.InnerExceptions.Count);

#if NETFRAMEWORK
            expectedMessage = generalMessage;
#endif
            Assert.AreEqual(expectedMessage, snEx.Message, "Wrong error Message");

            var index = 0;
            foreach (var exception in snEx.InnerExceptions)
            {
                var innerEx = (SignNowException) exception;
                var innerExMsg = $"inner-exception {index++}";

                Assert.AreEqual(innerExMsg, innerEx.Message);
            }
        }

        [TestMethod]
        public void ExceptionCanHandledAsAggregation()
        {
            var snExceptions = new List<SignNowException>();
            var aggregateMessage = new StringBuilder();

            for (var i = 0; i < 5; i++)
            {
                snExceptions.Add(
                    new SignNowException("message_" + i, HttpStatusCode.BadRequest));

                aggregateMessage.Append($" (message_{i})");
            }

            try
            {
                throw new SignNowException("test-error-message", snExceptions);
            }
            catch (AggregateException ex)
            {
                var expectedMessage = "test-error-message" + aggregateMessage;
#if NETFRAMEWORK
                expectedMessage = "test-error-message";
#endif
                Assert.AreEqual(expectedMessage, ex.Message);

                var msgPrefix = 0;
                foreach (var exception in ex.InnerExceptions)
                {
                    var snException = (SignNowException) exception;
                    Assert.AreEqual("message_" + msgPrefix, snException.Message);
                    Assert.AreEqual((int)HttpStatusCode.BadRequest, snException.Data["HttpStatusCode"]);
                    msgPrefix++;
                }
            }
        }
    }
}
