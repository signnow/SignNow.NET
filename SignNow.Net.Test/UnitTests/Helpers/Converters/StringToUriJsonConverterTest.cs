using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model.FieldTypes;

namespace UnitTests
{
    [TestClass]
    public class StringToUriJsonConverterTest
    {
        [DataTestMethod]
        [DataRow("http://signnow.com", DisplayName = "http valid Uri")]
        [DataRow("https://signnow.com", DisplayName = "https valid Uri")]
        [DataRow("ftp://signnow.com", DisplayName = "ftp valid Uri")]
        [DataRow("https://signnow.com/location/12345?param=42", DisplayName = "valid Uri with path and query")]
        public void ShouldSerializeValidUri(string location)
        {
            var testObj = new HyperlinkField()
            {
                Data = new Uri(location)
            };

            var testJson = JsonConvert.SerializeObject(testObj, Formatting.Indented);
            StringAssert.Contains(testJson, location);
        }

        [DataTestMethod]
        [DataRow("http://signnow.com", DisplayName = "http location")]
        [DataRow("https://signnow.com", DisplayName = "https location")]
        [DataRow("ftp://signnow.com", DisplayName = "ftp location")]
        [DataRow("http://signnow.com/api/1.0/doc/1?param=42", DisplayName = "location with path and query")]
        [DataRow(@"http:\/\/signnow.com\/api\/1.0\/doc\/1?param=42", DisplayName = "location with escaped path and query")]
        [DataRow(null, DisplayName = "nullable Uri")]
        public void ShouldDeserializeUriFromJsonString(string location)
        {
            var json = $"{{'data': '{location}'}}";

            var actual = JsonConvert.DeserializeObject<HyperlinkField>(json);
            Assert.AreEqual(location?.Replace(@"\/", "/"), actual.Data?.OriginalString);
        }
    }
}
