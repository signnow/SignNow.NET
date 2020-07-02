using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTests
{
    /// <summary>
    /// Extends Assertions to compare Json and Objects
    /// </summary>
    public static class AssertJson
    {
        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are not equal.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AreEqual(object expected, string actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected, Formatting.Indented);
            Assert.AreEqual(expectedJson, ToIndentedJson(actual));
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are not equal.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void AreEqual(string expected, object actual)
        {
            var actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);
            Assert.AreEqual(ToIndentedJson(expected), actualJson);
        }

        private static string ToIndentedJson(string json)
        {
            var asObject = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(asObject, Formatting.Indented);
        }
    }
}
