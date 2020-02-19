using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Internal.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class StringBase64ToByteArrayJsonConverterTest
    {
        [TestMethod]
        public void ShouldDeserializeBase64AsByteArray()
        {
            var testJson = JsonConvert.SerializeObject(new SignatureFaker().Generate(), Formatting.Indented);

            var actualObj = JsonConvert.DeserializeObject<Signature>(testJson);
            var actualJson = JsonConvert.SerializeObject(actualObj, Formatting.Indented);

            Assert.AreEqual(testJson, actualJson);
        }

        [TestMethod]
        public void CanConvertUriType()
        {
            var converter = new StringBase64ToByteArrayJsonConverter();
            Assert.IsTrue(converter.CanConvert(typeof(byte[])));
        }
    }
}