using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class StringToIntJsonConverterTest
    {
        [TestMethod]
        public void ShouldDeserializeAsInt()
        {
            var json = $"{{\"page_count\": \"100\"}}";
            var obj = JsonConvert.DeserializeObject<GetDocumentResponse>(json);

            Assert.AreEqual(100, obj.PageCount);
        }

        [TestMethod]
        public void ShouldSerializeIntegerNative()
        {
            var obj = new GetDocumentResponse
            {
                PageCount = 100
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
