using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace UnitTests
{
    public static class TestUtils
    {
        /// <inheritdoc cref="JsonConvert.SerializeObject(object?, Formatting)"/>
        public static string SerializeToJsonFormatted(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        /// <inheritdoc cref="JsonConvert.DeserializeObject{T}(string)"/>
        public static T DeserializeFromJson<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Dumps object as Json to console output
        /// </summary>
        /// <param name="value">any of object</param>
        public static void Dump(object value)
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
        public static string DynamicDataDisplayName(MethodInfo methodInfo, object[] data)
        {
            return $"{methodInfo?.Name} {data.FirstOrDefault()?.ToString()}";
        }
    }
}
