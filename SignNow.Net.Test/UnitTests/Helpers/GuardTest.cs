using System;
using Bogus.DataSets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                () => Guard.PropertyNotNull(doc.Id, "Document ID cannot be null."));

            Assert.AreEqual("Document ID cannot be null.", exception.Message);
        }

        [DataTestMethod]
        [DataRow("", DisplayName = "with empty string")]
        [DataRow(" ", DisplayName = "with whitespace string")]
        [DataRow(null, DisplayName = "with null")]
        public void ShouldGuardStringIsNotNullOrEmpty(string name)
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => Guard.ArgumentIsNotEmptyString(name, $"{nameof(name)} cannot be null, empty or whitespace"));

            Assert.AreEqual("name cannot be null, empty or whitespace", exception.Message);
        }
    }
}
