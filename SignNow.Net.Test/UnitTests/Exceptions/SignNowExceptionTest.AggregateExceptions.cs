using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Model;

namespace UnitTests
{
    [SuppressMessage("Microsoft.Globalization", "CA1303:Message string literals should be taken from resource file", Justification = "In the unit tests below no reason to use resource file")]
    public partial class SignNowExceptionTest
    {
        [TestMethod]
        public void Exception_Has_Proper_Data_Context()
        {
            var testException = new SignNowException(testMessage)
            {
                HttpStatusCode = HttpStatusCode.BadRequest
            };
            testException.Data["HttpStatusCode"] = (int)HttpStatusCode.BadRequest;
            testException.Data["Request"] = "Test Request Body";

            Assert.AreEqual(testMessage, testException.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, testException.HttpStatusCode);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, testException.Data["HttpStatusCode"]);
            Assert.AreEqual("Test Request Body", testException.Data["Request"]);
        }

        [TestMethod]
        [SuppressMessage("Microsoft.Globalization", "CA1305:StringBuilder.AppendFormat could vary based on locale", Justification = "Locale is not used for this test")]
        public void Exception_Contains_Exceptions_List()
        {
            var snExceptions = new List<SignNowException>();

            var snAggregateMsg = new StringBuilder("L1 Ex Message Example");

            for (int i = 0; i < 10; i++)
            {
                snExceptions.Add(new SignNowException($"inner-exception {i}"));
                snAggregateMsg.AppendFormat(" (inner-exception {0})", i);
            }

            var snEx  = new SignNowException("L1 Ex Message Example", snExceptions);

            Assert.AreEqual(10, snEx.InnerExceptions.Count);
            Assert.AreEqual(snAggregateMsg.ToString(), snEx.Message);


            var index = 0;
            foreach (SignNowException innerEx in snEx.InnerExceptions)
            {
                var innerExMsg = String.Format("inner-exception {0}", index++);

                Assert.AreEqual(innerExMsg, innerEx.Message);
            }
        }

        [TestMethod]
        public void Exception_Handled_As_Aggregation()
        {
            var snExceptions = new List<SignNowException>();
            var aggregateMessage = string.Empty;

            for (var i = 0; i < 5; i++)
            {
                snExceptions.Add(
                    new SignNowException("message_" + i, HttpStatusCode.BadRequest));

                aggregateMessage += " (message_" + i + ")";
            }

            try
            {
                throw new SignNowException("test-error-message", snExceptions);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual("test-error-message" + aggregateMessage, ex.Message);

                var msgPrefix = 0;
                foreach (SignNowException snException in ex.InnerExceptions)
                {
                    Assert.AreEqual("message_" + msgPrefix, snException.Message);
                    Assert.AreEqual((int)HttpStatusCode.BadRequest, snException.Data["HttpStatusCode"]);
                    msgPrefix++;
                }
            }
        }
    }
}
