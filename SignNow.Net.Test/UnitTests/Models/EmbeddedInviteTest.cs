using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Model;

namespace UnitTests.Models
{
    [TestClass]
    public class EmbeddedInviteTest : SignNowTestBase
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

        [TestMethod]
        public void ThrowsExceptionForSignatureFieldCharsMore255Symbols()
        {
            var invite = new EmbeddedInvite();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => invite.PrefillSignatureName = Faker.Random.String(300));

            StringAssert.Contains(exception.Message, "Prefilled text in the Signature field can be maximum 255 characters.");
            Assert.AreEqual("PrefillSignatureName", exception.ParamName);
        }

        [TestMethod]
        public void ThrowsExceptionIfRequiredSignatureNameEnabled()
        {
            var invite = new EmbeddedInvite
            {
                RequiredPresetSignatureName = "test"
            };

            var exception = Assert.ThrowsException<ArgumentException>(
                () => invite.PrefillSignatureName = "prefill");

            StringAssert.Contains(exception.Message, "Required preset for Signature name is set. Cannot be used together with");
            Assert.AreEqual("PrefillSignatureName", exception.ParamName);

            exception = Assert.ThrowsException<ArgumentException>(
                () => invite.ForceNewSignature = true);

            StringAssert.Contains(exception.Message, "Required preset for Signature name is set. Cannot be used together with");
            Assert.AreEqual("ForceNewSignature", exception.ParamName);
        }

        [TestMethod]
        public void ThrowsExceptionIfPrefilledSignatureName()
        {
            var invite = new EmbeddedInvite
            {
                PrefillSignatureName = "test"
            };

            var exception = Assert.ThrowsException<ArgumentException>(
                () => invite.RequiredPresetSignatureName = "prefill");

            StringAssert.Contains(exception.Message, "Prefill for Signature name or Force new signature is set. Cannot be used together with");
            Assert.AreEqual("RequiredPresetSignatureName", exception.ParamName);
        }
    }
}
