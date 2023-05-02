using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;

namespace UnitTests.Models
{
    [TestClass]
    public class EmbeddedInviteTest
    {
        [TestMethod]
        public void ThrowsExceptionForInvalidEmail()
        {
            var invite = new EmbeddedInvite();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => invite.Email = "not-an-email.com");

            var errorMessage = string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfEmail, "not-an-email.com");
            StringAssert.Contains(exception.Message, errorMessage);
            Assert.AreEqual("not-an-email.com", exception.ParamName);
        }

        [TestMethod]
        public void ThrowsExceptionForNotAllowedRange()
        {
            var invite = new EmbeddedInvite();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => invite.SigningOrder = 0);

            StringAssert.Contains(exception.Message, "Value cannot be 0");
            Assert.AreEqual("SigningOrder", exception.ParamName);
        }
    }
}
