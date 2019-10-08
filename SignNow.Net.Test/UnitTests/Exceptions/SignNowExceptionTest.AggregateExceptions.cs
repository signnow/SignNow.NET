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
        public void Exception_Has_Proper_Data_Context()
        {
            var testException  = new SignNowException(testMessage);

            testException.HttpStatusCode = HttpStatusCode.BadRequest;
            testException.Data["HttpStatusCode"] = (int)HttpStatusCode.BadRequest;
            testException.Data["Request"] = "Test Request Body";

            Assert.AreEqual(testMessage, testException.Message);
            Assert.AreEqual(HttpStatusCode.BadRequest, testException.HttpStatusCode);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, testException.Data["HttpStatusCode"]);
            Assert.AreEqual("Test Request Body", testException.Data["Request"]);
        }

        [TestMethod]
        public void Exception_Contains_Exceptions_List()
        {
            var nativeExceptions = new List<Exception>();
            var snExceptions     = new List<SignNowException>();

            var snAggregateMsg = new StringBuilder("L1 Ex Message Example");

            for (int i = 0; i < 10; i++)
            {
                nativeExceptions.Add(new Exception($"inner-exception {i}"));
                snExceptions.Add(new SignNowException($"inner-exception {i}"));
                snAggregateMsg.AppendFormat(" (inner-exception {0})", i);
            }

            var natEx = new SignNowException("L1 Ex Message Example", nativeExceptions);
            var snEx  = new SignNowException("L1 Ex Message Example", snExceptions);

            Assert.AreEqual(10, natEx.InnerExceptions.Count);
            Assert.AreEqual(10, snEx.InnerExceptions.Count);

            Assert.AreEqual(snAggregateMsg.ToString(), natEx.Message);
            Assert.AreEqual(snAggregateMsg.ToString(), snEx.Message);


            var index = 0;
            foreach (Exception innerEx in natEx.InnerExceptions)
            {
                var innerExMsg = String.Format("inner-exception {0}", index++);

                Assert.AreEqual(innerExMsg, innerEx.Message);
            }

            index = 0;
            foreach (SignNowException innerEx in snEx.InnerExceptions)
            {
                var innerExMsg = String.Format("inner-exception {0}", index++);

                Assert.AreEqual(innerExMsg, innerEx.Message);
            }
        }
    }
}
