using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.Requests;

namespace UnitTests.Requests
{
    [TestClass]
    public class LimitOffsetOptionsTest
    {
        [TestMethod]
        public void BuildQuery()
        {
            var emptyOptions = new LimitOffsetOptions();
            var fullOptions = new LimitOffsetOptions
            {
                Limit = 2,
                Offset = 3
            };

            Assert.AreEqual(String.Empty, emptyOptions.ToQueryString());
            Assert.AreEqual("limit=2&offset=3", fullOptions.ToQueryString());
        }
    }
}
