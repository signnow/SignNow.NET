using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model.FieldContents;

namespace UnitTests
{
    [TestClass]
    public class StringToUriJsonConverterTest
    {
        [DataTestMethod]
        [DataRow("",                    DisplayName = "empty url")]
        [DataRow("http://signnow.com",  DisplayName = "http valid Uri")]
        [DataRow("https://signnow.com", DisplayName = "https valid Uri")]
        [DataRow("ftp://signnow.com",   DisplayName = "ftp valid Uri")]
        [DataRow("https://signnow.com/location/12345?param=42", DisplayName = "valid Uri with path and query")]
        public void ShouldSerializeValidUri(string location)
        {
            var testObj = new HyperlinkContent();

            if (!string.IsNullOrEmpty(location))
            {
                testObj.Data = new Uri(location);
            };

            var testJson = JsonConvert.SerializeObject(testObj, Formatting.Indented);
            StringAssert.Contains(testJson, location);
        }

        [DataTestMethod]
        [DataRow("http://signnow.com",  DisplayName = "http location")]
        [DataRow("https://signnow.com", DisplayName = "https location")]
        [DataRow("ftp://signnow.com",   DisplayName = "ftp location")]
        [DataRow("http://signnow.com/api/1.0/doc/1?param=42",        DisplayName = "location with path and query")]
        [DataRow(@"http:\/\/signnow.com\/api\/1.0\/doc\/1?param=42", DisplayName = "location with escaped path and query")]
        public void ShouldDeserializeUriFromJsonString(string location)
        {
            var json = $"{{'data': '{location}'}}";

            var actual = JsonConvert.DeserializeObject<HyperlinkContent>(json);
            Assert.AreEqual(location?.Replace(@"\/", "/"), actual.Data?.OriginalString);
        }

        [TestMethod]
        public void ShouldThrowExceptionForBrokenUrl()
        {
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => JsonConvert.DeserializeObject<HyperlinkContent>("{'data': '42'}"));

            Assert.AreEqual("Unexpected value when converting to Uri. Expected an absolute Url, got '42'.", exception.Message);
        }

        [TestMethod]
        public void CanConvertUriType()
        {
            var converter = new StringToUriJsonConverter();
            Assert.IsTrue(converter.CanConvert(typeof(Uri)));
        }
    }
}
