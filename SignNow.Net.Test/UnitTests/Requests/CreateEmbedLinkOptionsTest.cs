using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model.Requests;

namespace UnitTests
{
    [TestClass]
    public class CreateEmbedLinkOptionsTest
    {
        [TestMethod]
        public void ThrowsExceptionForNotValidTimeRange()
        {
            var options = new CreateEmbedLinkOptions();

            var exception1 = Assert.ThrowsException<ArgumentException>(
                () => options.LinkExpiration = 0);

            var exception2 = Assert.ThrowsException<ArgumentException>(
                () => options.LinkExpiration = 46);

            Assert.IsNull(options.LinkExpiration);
            StringAssert.Contains(exception1.Message, ExceptionMessages.AllowedRangeMustBeFrom15to45);
            StringAssert.Contains(exception2.Message, ExceptionMessages.AllowedRangeMustBeFrom15to45);
        }
    }
}
