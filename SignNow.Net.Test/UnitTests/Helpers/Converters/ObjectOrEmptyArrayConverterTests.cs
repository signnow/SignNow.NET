using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net._Internal.Helpers.Converters;

namespace SignNow.Net.Test.UnitTests.Helpers.Converters
{
    [TestClass]
    public class ObjectOrEmptyArrayConverterTests
    {
        public class TestContainer
        {
            [JsonProperty("version")]
            public int Version { get; set; }

            [JsonProperty("test_property")]
            [JsonConverter(typeof(ObjectOrEmptyArrayConverter))]
            public TestModel TestProperty { get; set; }

            public class TestModel
            {
                [JsonProperty("property1")]
                public string Property1 { get; set; }

                [JsonProperty("property2")]
                public string Property2 { get; set; }
            }
        }

        [TestMethod]
        [DataRow("{'version': 1, 'test_property': {} }")]
        [DataRow("{'version': 1, 'test_property': {'property1': 'a'} }")]
        [DataRow("{'version': 1, 'test_property': {'property1': 'a', 'property2': 'b'} }")]
        public void ReadJson_ShouldDeserializeToTargetType(string json)
        {
            var result = JsonConvert.DeserializeObject<TestContainer>(json);

            Assert.IsInstanceOfType(result.TestProperty, typeof(TestContainer.TestModel));
        }

        [TestMethod]
        [DataRow("{'version': 1, 'test_property': [] }")]
        [DataRow("{'version': 1, 'test_property': null }")]
        public void ReadJson_ShouldDeserializeToNull(string json)
        {
            var result = JsonConvert.DeserializeObject<TestContainer>(json);

            Assert.IsNull(result.TestProperty);
        }

        [TestMethod]
        [DataRow("{'version': 1, 'test_property': 1 }")]
        [DataRow("{'version': 1, 'test_property': true }")]
        [DataRow("{'version': 1, 'test_property': ['a', 'b'] }")]
        public void ReadJson_ShouldThrowJsonDeserilizationException(string json)
        {
            Assert.ThrowsException<JsonSerializationException>(
                () => JsonConvert.DeserializeObject<TestContainer>(json)
            );
        }
    }
}
