using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTests
{
    /// <summary>
    /// Extends Assertions for more comfortable Unit tests assertions
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

        /// <summary>
        /// Tests whether the specified Stream is a PDF file and throws an exception
        /// if it is not a PDF.
        /// </summary>
        /// <param name="file">The Stream the test expects to be a PDF file.</param>
        /// <param name="message">
        /// The message to include in the exception when <paramref name="file"/>
        /// is not a PDF file. The message is shown in test results.
        /// </param>
        public static void StreamIsPdf(this Assert assert, Stream file, string message = "")
        {
            Assert.IsNotNull(file, "Document is Empty or not exists");
            Assert.IsTrue(file.CanRead, "Not readable Document content");

            var msg = String.IsNullOrEmpty(message) ? "Document content is not a PDF format" : message;
            var pdfSignature = "%PDF-1.";
            string actual;

            using var reader = new StreamReader(file, Encoding.UTF8);
            actual = reader.ReadLine();

            StringAssert.StartsWith(actual, pdfSignature, msg);
        }
    }
}
