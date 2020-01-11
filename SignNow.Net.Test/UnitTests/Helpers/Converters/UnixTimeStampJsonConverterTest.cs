using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class UnixTimeStampJsonConverterTest
    {
        private readonly string _dtFormat = "dd/MM/yyyy H:mm:ss";

        [TestMethod]
        public void ShouldDeserializeAsDateTime()
        {
            // Should deserialize from string UTC format
            var jsonUtc = $"{{\"created\":\"1572968651\"}}";
            var objUtc = JsonConvert.DeserializeObject<GetDocumentResponse>(jsonUtc);
            var expectedUtc = DateTime.ParseExact("05/11/2019 15:44:11", _dtFormat, null);

            Assert.AreEqual(expectedUtc, objUtc.Created);
        }

        [TestMethod]
        public void ShouldSerializeDateTimeNative()
        {
            var testDate = new DateTime(2019, 11, 5, 15, 44, 11, DateTimeKind.Utc);

            var obj = new GetDocumentResponse
            {
                Created = testDate,
                Updated = testDate
            };

            var actual = JsonConvert.SerializeObject(obj);
            var expected = $"\"created\":1572968651,\"updated\":1572968651";

            StringAssert.Contains(actual, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(JsonSerializationException))]
        public void ThrowsExceptionOnUnixIncompatibleDate()
        {
            // Cannot convert date value that is before Unix epoch of 00:00:00 UTC on 1 January 1970
            var obj = new GetDocumentResponse
            {
                Created = default,
                Updated = new DateTime()
            };

            JsonConvert.SerializeObject(obj);
        }

        [TestMethod]
        public void CanConvertDateTimeType()
        {
            var converter = new UnixTimeStampJsonConverter();
            Assert.IsTrue(converter.CanConvert(typeof(DateTime)));
        }
    }
}
