using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace UnitTests.Helpers.Converters
{
    [TestClass]
    public class StringToIntJsonConverterTest
    {
        [TestMethod]
        public void ShouldDeserializeAsInt()
        {
            var json = $"{{\"page_count\": \"100\"}}";
            var obj = JsonConvert.DeserializeObject<SignNowDocument>(json);

            Assert.AreEqual(100, obj.PageCount);
        }

        [TestMethod]
        public void ShouldSerializeIntegerNative()
        {
            var obj = new SignNowDocument
            {
                PageCount = 100,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };

            var actual = JsonConvert.SerializeObject(obj);
            var expected = $"\"page_count\":100";

            StringAssert.Contains(actual, expected);
        }

        [TestMethod]
        public void CanConvertIntType()
        {
            var converter = new StringToIntJsonConverter();
            Assert.IsTrue(converter.CanConvert(typeof(int)));
        }
    }
}
