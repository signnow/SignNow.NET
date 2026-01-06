using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net._Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace UnitTests.Helpers.Converters
{
    [TestClass]
    public class DurationToTimeSpanConverterTest
    {
        [DataTestMethod]
        [DataRow(@"{'duration': 3}", 3)]
        [DataRow(@"{'duration': 0.05}", 0.05)]
        [DataRow(@"{'duration': null}", 0)]
        public void ShouldDeserializeAsTimeSpan(string json, double expected)
        {
            var callback = TestUtils.DeserializeFromJson<Callback<CallbackContentAllFields>>(json);

            Assert.AreEqual(TimeSpan.FromSeconds(expected), callback.Duration);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(3)]
        [DataRow(0.03)]
        public void ShouldSerializeTimeSpanAsSeconds(double seconds)
        {
            var callback = new Callback<CallbackContentAllFields>
            {
                Duration = TimeSpan.FromSeconds(seconds)
            };

            StringAssert.Contains(
                TestUtils.SerializeToJsonFormatted(callback),
                $"\"duration\": {seconds}"
            );
        }

        [TestMethod]
        public void CanConvertTimeSpanType()
        {
            var converter = new DurationToTimeSpanConverter();

            Assert.IsTrue(converter.CanConvert(typeof(TimeSpan)));
            Assert.IsFalse(converter.CanConvert(typeof(int)));
        }

        [TestMethod]
        public void ThrowExceptionForNotSupportedTypes()
        {
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => TestUtils.DeserializeFromJson<Callback<CallbackContentAllFields>>("{'duration':'invalid_string'}"));

            Assert.AreEqual("Unexpected value when converting to `TimeSpan`. Expected `Integer`, `Float`, got String.", exception.Message);
        }
    }
}
