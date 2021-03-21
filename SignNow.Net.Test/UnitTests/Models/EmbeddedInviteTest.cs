using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;

namespace UnitTests
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

            Assert.AreEqual(
                String.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfEmail, "not-an-email.com"),
                exception.Message);
        }

        [TestMethod]
        public void ThrowsExceptionForNotAllowedRange()
        {
            var invite = new EmbeddedInvite();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => invite.SigningOrder = 0);

            StringAssert.Contains(exception.Message, "Value cannot be 0");
        }
    }
}
