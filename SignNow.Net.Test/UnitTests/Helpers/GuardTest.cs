using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Helpers;
using SignNow.Net.Model;
using SignNow.Net.Test.Constants;

namespace UnitTests
{
    [TestClass]
    public class GuardTest
    {
        [TestMethod]
        public void ShouldGuardArgumentNullException()
        {
            Object nullableObj = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => Guard.ArgumentNotNull(nullableObj, nameof(nullableObj)));

            StringAssert.Contains(exception.Message, ErrorMessages.ValueCannotBeNull);
            StringAssert.Contains(exception.ParamName, "nullableObj");
        }

        [TestMethod]
        public void ShouldGuardPropertyNullException()
        {
            var doc = new SignNowDocument();

            var exception = Assert.ThrowsException<ArgumentException>(
                () => Guard.PropertyNotNull(doc.Id, nameof(doc.Id), ExceptionMessages.RequestUrlIsNull));

            Assert.AreEqual($"{ExceptionMessages.RequestUrlIsNull} (Parameter 'Id')", exception.Message);
            Assert.AreEqual("Id", exception.ParamName);
        }

        [DataTestMethod]
        [DataRow("", DisplayName = "with empty string")]
        [DataRow(" ", DisplayName = "with whitespace string")]
        [DataRow(null, DisplayName = "with null")]
        public void ShouldGuardStringIsNotNullOrEmpty(string name)
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => Guard.ArgumentIsNotEmptyString(name, nameof(name)));

            Assert.AreEqual($"{ExceptionMessages.StringNotNullOrEmptyOrWhitespace} (Parameter 'name')", exception.Message);
            Assert.AreEqual("name", exception.ParamName);
        }
    }
}
