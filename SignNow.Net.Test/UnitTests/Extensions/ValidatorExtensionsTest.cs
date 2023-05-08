using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Extensions;

namespace UnitTests.Extensions
{
    [TestClass]
    public class ValidatorExtensionsTest
    {
        [DataTestMethod]
        [DataRow("da057e65e97c9fa6b96485ca4970260f309580019999", DisplayName = "more than 40 chars long")]
        [DataRow("da057e65e97c9fa6b9648", DisplayName = "less than 40 chars long")]
        [DataRow("test", DisplayName = "short string")]
        [DataRow("#", DisplayName = "one character")]
        [DataRow("--test-----------^----------------------", DisplayName = "40 chars with forbidden char")]
        [DataRow("0000000000000000000000000000000000000000 ", DisplayName = "40 chars with trailing space")]
        public void ShouldValidateDocumentId(string id)
        {
            var exception = Assert.ThrowsException<ArgumentException>(id.ValidateId);

            var errorMessage = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfId, id);
            StringAssert.Contains(exception.Message, errorMessage);
            Assert.AreEqual(id, exception.ParamName);
        }

        [DataTestMethod]
        [DataRow("user1@", DisplayName = "without domain name")]
        [DataRow("user1@localhost.com.", DisplayName = "dot at the end of the domain part")]
        [DataRow("user1@localhost..com", DisplayName = "two dots in the domain part")]
        [DataRow("user1.@localhost.com", DisplayName = "dot at the end of the local part")]
        [DataRow(".user1@localhost.com", DisplayName = "dot at the beginning of the local part")]
        [DataRow("user1..@localhost.com", DisplayName = "two dots in the local part")]
        [DataRow("user1%@localhost.com", DisplayName = "not valid char in local part")]
        [DataRow("user1@localhost-.com", DisplayName = "dash in the end of domain part")]
        [DataRow("user1@-localhost.com", DisplayName = "dash at the beginning of domain part")]
        [DataRow("user1@noreply.com\r", DisplayName = "extra return line symbols")]
        public void ShouldFailNonValidEmails(string email)
        {
            var exception = Assert.ThrowsException<ArgumentException>(email.ValidateEmail);

            var errorMessage = string.Format(CultureInfo.InvariantCulture, ExceptionMessages.InvalidFormatOfEmail, email);
            StringAssert.Contains(exception.Message, errorMessage);
            Assert.AreEqual(email, exception.ParamName);
        }

        [DataTestMethod]
        [DataRow("user@email.com")]
        [DataRow("user1@email.name")]
        [DataRow("user+test@email.org")]
        [DataRow("user-test@email.org")]
        [DataRow("user'test@email.org")]
        [DataRow("user_test@email.org")]
        [DataRow("user.test@email.org")]
        [DataRow("user.test_1@email.org")]
        [DataRow("user.test_1@no-email.org")]
        [DataRow("user@192.168.0.1")]
        [DataRow("user@10.1.100.1")]
        public void ShouldPassValidEmails(string email)
        {
            Assert.AreEqual(email, email.ValidateEmail());
        }
    }
}
