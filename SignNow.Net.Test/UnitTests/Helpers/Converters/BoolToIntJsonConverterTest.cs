using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace UnitTests.Helpers.Converters
{
    [TestClass]
    public class BoolToIntJsonConverterTest
    {
        [DataTestMethod]
        [DataRow(1, true, DisplayName = "Boolean True for integer 1")]
        [DataRow(0, false, DisplayName = "Boolean False for integer 0")]
        [DataRow(99, false, DisplayName = "False for any integer not equals to 1")]
        public void ShouldDeserializeAsBoolean(int jsonParam, bool expected)
        {
            var json = $@"{{
                ""force_new_signature"": ""{jsonParam}"",
                ""email"": ""test@signnow.com"",
                ""role"": {{
                    ""unique_id"": ""xxx"",
                    ""signing_order"": 1,
                    ""name"": ""unit-test""
                }}
            }}";
            var obj = JsonConvert.DeserializeObject<SignerOptions>(json);

            Assert.AreEqual(expected, obj.ForceNewSignature);
        }

        [TestMethod]
        public void ShouldSerializeBooleanTypeAsInteger()
        {
            var obj = new SignerOptions("test@signnow.com", new Role())
            {
                ForceNewSignature = true,
                AllowToReassign = false
            };

            var actual = JsonConvert.SerializeObject(obj);

            StringAssert.Contains(actual, $"\"force_new_signature\":1");
            StringAssert.Contains(actual, $"\"reassign\":0");
        }

        [TestMethod]
        public void CanConvertBooleanType()
        {
            var converter = new BoolToIntJsonConverter();
            Assert.IsTrue(converter.CanConvert(typeof(bool)));
        }
    }
}
