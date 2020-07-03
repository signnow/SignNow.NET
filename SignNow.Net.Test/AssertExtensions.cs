using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTests
{
    /// <summary>
    /// Extends Assertions to compare Json and Objects
    /// </summary>
    public static class AssertExtensions
    {
        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are not equal.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void JsonEqual(this Assert assert, object expected, string actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected, Formatting.Indented);
            Assert.AreEqual(expectedJson, PrettifyJson(actual));
        }

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are not equal.
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void JsonEqual(this Assert assert, string expected, object actual)
        {
            var actualJson = JsonConvert.SerializeObject(actual, Formatting.Indented);
            Assert.AreEqual(PrettifyJson(expected), actualJson);
        }

        private static string PrettifyJson(string json)
        {
            using var stringReader = new StringReader(json);
            using var stringWriter = new StringWriter();
            using var jsonReader = new JsonTextReader(stringReader);
            using var jsonWriter = new JsonTextWriter(stringWriter)
            {
                Formatting = Formatting.Indented
            };
            jsonWriter.WriteToken(jsonReader);

            return stringWriter.ToString();
        }
    }
}
