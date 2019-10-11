using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Model;

namespace UnitTests
{
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
        [DataRow("{\"error\": \"invalid_request\"}", HttpStatusCode.BadRequest)]
        [DataRow("{\"error\": \"invalid_token\",\"code\": 1537}", HttpStatusCode.BadRequest)]
        [DataRow("{\"errors\": [{\"code\": 65541,\"message\": \"internal api error\"}]}", HttpStatusCode.BadRequest)]
        [DataRow("{\"404\": \"Unable to find a route to match the URI: event_subscription\"}", HttpStatusCode.BadRequest)]
        public void Exception_Handled_As_Aggregation(string errors, HttpStatusCode statusCode)
        {
            var items = 3;

            var snExceptions = new List<SignNowException>();
            var erroredTasks = new System.Threading.Tasks.Task[items];
            var errorMsg = JsonConvert.DeserializeObject<ErrorResponse>(errors).GetErrorMessage();

            Action<string, HttpStatusCode> responseJob = (string err, HttpStatusCode httpStatus) =>
            {
                throw new SignNowException(errorMsg, httpStatus);
            };

            for (var i = 0; i < items; i++)
            {
                var jobid = i;
                erroredTasks[i] = System.Threading.Tasks.Task.Run( () => responseJob(errors, statusCode) );
            }

            try
            {
                System.Threading.Tasks.Task.WaitAll(erroredTasks);
            }
            catch (AggregateException aEx)
            {
                foreach (SignNowException ex in aEx.InnerExceptions)
                {
                    Assert.AreEqual(errorMsg, ex.Message);
                    Assert.AreEqual((int)statusCode, ex.Data["HttpStatusCode"]);

                    snExceptions.Add(ex);
                }
            }

            try
            {
                throw new SignNowException("CollectedExceptions", snExceptions);
            }
            catch (SignNowException snExc)
            {
                foreach (SignNowException ex in snExc.InnerExceptions)
                {
                    Assert.AreEqual(errorMsg, ex.Message);
                }

                var expectedMsg = String.Format("CollectedExceptions ({0}) ({0}) ({0})", errorMsg);

                Assert.AreEqual(expectedMsg, snExc.Message);
            }
        }
    }
}
