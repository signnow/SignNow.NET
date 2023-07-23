using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;
using SignNow.Net.Model.FieldContents;

namespace UnitTests.Helpers.Converters
{
    [TestClass]
    public class StringToUriJsonConverterTest
    {
        [DataTestMethod]
        [DataRow("", DisplayName = "empty url")]
        [DataRow("http://signnow.com", DisplayName = "http valid Uri")]
        [DataRow("https://signnow.com", DisplayName = "https valid Uri")]
        [DataRow("ftp://signnow.com", DisplayName = "ftp valid Uri")]
        [DataRow("https://signnow.com/location/12345?param=42", DisplayName = "valid Uri with path and query")]
        public void ShouldSerializeValidUri(string location)
        {
            var testObj = new HyperlinkContent();
            var data = $"'data': '{location}'";

            if (!string.IsNullOrEmpty(location))
            {
                testObj.Data = new Uri(location);
            }
            else
            {
                testObj.Data = null;
                data = "'data': null";
            }

            var expected = @$"{{
                'email': null,
                'label': null,
                {data},
                'id': null,
                'user_id': null,
                'page_number': 0
            }}";

            Assert.That.JsonEqual(expected, testObj);
        }

        [DataTestMethod]
        [DataRow("http://signnow.com", DisplayName = "http location")]
        [DataRow("https://signnow.com", DisplayName = "https location")]
        [DataRow("ftp://signnow.com", DisplayName = "ftp location")]
        [DataRow("http://signnow.com/api/1.0/doc/1?param=42", DisplayName = "location with path and query")]
        [DataRow(@"http:\/\/signnow.com\/api\/1.0\/doc\/1?param=42",
            DisplayName = "location with escaped path and query")]
        public void ShouldDeserializeUriFromJsonString(string location)
        {
            var json = $"{{'data': '{location}'}}";

            var actual = JsonConvert.DeserializeObject<HyperlinkContent>(json);
            Assert.AreEqual(location?.Replace(@"\/", "/"), actual.Data?.OriginalString);
        }

        [TestMethod]
        public void ShouldBeSerializable()
        {
            var emptyClass = new HyperlinkContent();
            var expected = @"{
                'email': null,
                'label': null,
                'data': null,
                'id': null,
                'user_id': null,
                'page_number': 0
            }";

            Assert.That.JsonEqual(expected, emptyClass);
        }

        [TestMethod]
        public void ShouldThrowExceptionForBrokenUrl()
        {
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => JsonConvert.DeserializeObject<HyperlinkContent>(@"{""data"": ""42""}"));

            var expectedMessage = string.Format(CultureInfo.CurrentCulture,
                ExceptionMessages.UnexpectedValueWhenConverting,
                "Uri", "an absolute Url", 42);

            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [TestMethod]
        public void CanConvertUriType()
        {
            var converter = new StringToUriJsonConverter();

            Assert.IsTrue(converter.CanConvert(typeof(Uri)));
            Assert.IsFalse(converter.CanConvert(typeof(string)));
        }

        [TestMethod]
        public void ShouldSerializeNullableUrls()
        {
            var testObj = new PageLinks();

            Assert.That.JsonEqual("{}", testObj);

            testObj.Next = new Uri("https://signnow.com/api/v2/events?page=2");

            var expectedNext = $@"{{
                ""next"": ""https://signnow.com/api/v2/events?page=2""
            }}";

            Assert.That.JsonEqual(expectedNext, testObj);

            testObj.Next = null;
            testObj.Previous = new Uri("https://signnow.com/api/v2/events?page=2");

            var expectedPrevious = $@"{{
                ""previous"": ""https://signnow.com/api/v2/events?page=2""
            }}";

            Assert.That.JsonEqual(expectedPrevious, testObj);
        }
    }
}
