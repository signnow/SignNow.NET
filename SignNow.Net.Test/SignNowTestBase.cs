using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace SignNow.Net.Test
{
    public abstract class SignNowTestBase
    {
        public virtual Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");
        public virtual Uri ApplicationBaseUrl => new Uri("https://app-eval.signnow.com/");

        private readonly string baseTestDataPath = "../../../TestData/".Replace('/', Path.DirectorySeparatorChar);

#pragma warning disable CA1051 // Do not declare visible instance fields [SignNow.Net.Test]csharp(CA1051)
        protected readonly string pdfFileName = "DocumentUpload.pdf";
        protected readonly string txtFileName = "DocumentUpload.txt";
#pragma warning restore CA1051 // Do not declare visible instance fields

        protected string PdfFilePath => Path.Combine($"{baseTestDataPath}Documents", pdfFileName);
        protected string TxtFilePath => Path.Combine($"{baseTestDataPath}Documents", txtFileName);

        /// <inheritdoc cref="JsonConvert.SerializeObject(object?, Formatting)"/>
        protected static string SerializeToJsonFormatted(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        /// <inheritdoc cref="JsonConvert.DeserializeObject{T}(string)"/>
        protected static T DeserializeFromJson<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Dumps object as Json to console output
        /// </summary>
        /// <param name="value">any of object</param>
        protected static void Dump(object value)
        {
            Console.WriteLine(SerializeToJsonFormatted(value));
        }

        /// <summary>
        /// Retrieve test case name for `DataTestMethod`.
        /// Test name must be a first element in `object`.
        /// <example>
        /// <code>
        /// public static IEnumerable{object[]} FieldContentProvider()
        /// {
        ///    // Test DisplayName | test object
        ///    yield return new object[] { "radiobutton content test", new RadiobuttonContentFaker().Generate() };
        /// }
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected static string DynamicDataDisplayName(MethodInfo methodInfo, object[] data)
        {
            return $"{methodInfo?.Name} {data.FirstOrDefault()?.ToString()}";
        }
    }
}
