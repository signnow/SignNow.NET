using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace UnitTests.Helpers.Converters
{
    [TestClass]
    public class UnixTimeStampJsonConverterTest
    {
        private readonly string _dtFormat = "dd/MM/yyyy H:mm:ss";

        [DataTestMethod]
        [DataRow(@"{'created':'1572968651'}", DisplayName = "with timestamp as string")]
        [DataRow(@"{'created': 1572968651 }", DisplayName = "with timestamp as integer")]
        public void ShouldDeserializeAsDateTime(string input)
        {
            var objUtc = JsonConvert.DeserializeObject<SignNowDocument>(input);
            var expectedUtc = DateTime.ParseExact("05/11/2019 15:44:11", _dtFormat, null);

            Assert.AreEqual(expectedUtc, objUtc.Created);
        }

        [TestMethod]
        public void ShouldSerializeDateTimeNative()
        {
            var testDate = new DateTime(2019, 11, 5, 15, 44, 11, DateTimeKind.Utc);

            var obj = new SignNowDocument
            {
                Created = testDate,
                Updated = testDate
            };

            var actual = JsonConvert.SerializeObject(obj);
            const string Expected = "\"created\":\"1572968651\",\"updated\":\"1572968651\"";

            StringAssert.Contains(actual, Expected);
        }

        [TestMethod]
        public void CanConvertDateTimeType()
        {
            var converter = new UnixTimeStampJsonConverter();

            Assert.IsTrue(converter.CanConvert(typeof(DateTime)));
            Assert.IsFalse(converter.CanConvert(typeof(long)));
        }

        [TestMethod]
        public void ThrowExceptionForNotSupportedTypes()
        {
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => JsonConvert.DeserializeObject<SignNowDocument>("{'created':1.2}"));

            var expectedMessage = string.Format(CultureInfo.CurrentCulture, ExceptionMessages.UnexpectedValueWhenConverting,
                "DateTime", "`String`, `Integer`", "Double");

            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}
