using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Exceptions;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace UnitTests
{
    [TestClass]
    public class StringToBoolJsonConverterTest
    {
        [DataTestMethod]
        [DataRow("true", true, DisplayName = "Boolean TRUE as string")]
        [DataRow("false", false, DisplayName = "Boolean FALSE as string")]
        [DataRow("1", true, DisplayName = "Boolean 1 as string")]
        [DataRow("0", false, DisplayName = "Boolean 0 as string")]
        [DataRow("TRUE", true, DisplayName = "Boolean TRUE as Uppercase string")]
        [DataRow("fAlSe", false, DisplayName = "Boolean FALSE as Mixed case string")]
        public void ShouldDeserializeAsBoolean(string jsonParam, bool expected)
        {
            var json = $"{{\"active\": \"{jsonParam}\"}}";
            var obj = JsonConvert.DeserializeObject<User>(json);

            Assert.AreEqual(expected, obj.Active);
        }

        [TestMethod]
        public void ThrowsExceptionOnWrongValue()
        {
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => JsonConvert.DeserializeObject<User>($"{{\"active\": \"error\"}}"));

            var expectedMessage = string.Format(CultureInfo.CurrentCulture, ExceptionMessages.UnexpectedValueWhenConverting,
                "Boolean", "`true`, `false`", "error");

            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [DataTestMethod]
        [DataRow(true, DisplayName = "Boolean true")]
        [DataRow(false, DisplayName = "Boolean false")]
        [SuppressMessage("[CA1308] Replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", "CA1308")]
        public void ShouldSerializeBooleanTypeNative(bool param)
        {
            var obj = new User
            {
                Active = param
            };

            var actual = JsonConvert.SerializeObject(obj);
            var expected = $"\"active\":{param.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()}";

            StringAssert.Contains(actual, expected);
        }

        [TestMethod]
        public void CanConvertDateTimeType()
        {
            var converter = new StringToBoolJsonConverter();
            Assert.IsTrue(converter.CanConvert(typeof(bool)));
        }
    }
}
