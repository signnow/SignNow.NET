using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SignNow.Net.Test.Extensions
{
    public static class FixtureExtensions
    {
        public static readonly string FixturePath = "../../../TestData/Fixtures".Replace('/', Path.DirectorySeparatorChar);

        /// <summary>
        /// Reads fixture from json file as string.
        /// </summary>
        /// <param name="fixtureName"><see cref="Constants.JsonFixtures" /> relative path</param>
        /// <returns>sting contents of json fixture</returns>
        public static string AsJsonSting(this string fixtureName)
        {
            return ReadFixture(fixtureName);
        }

        /// <summary>
        /// Creates Json object from fixture json file.
        /// </summary>
        /// <param name="fixtureName"><see cref="Constants.JsonFixtures"/> relative path</param>
        /// <returns><see cref="JObject"/></returns>
        public static JObject AsJsonObject(this string fixtureName)
        {
            return JObject.Parse(ReadFixture(fixtureName));
        }

        /// <summary>
        /// Reads json fixtures and returns string content.
        /// </summary>
        /// <param name="path">Relative fixture path.</param>
        /// <example>
        ///     var json = ReadFixture(@"../relativePathTo/Document/fixtureName.json");
        /// </example>
        /// <returns></returns>
        private static string ReadFixture(string path)
        {
            var fullPath = path?.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

            return new StreamReader(fullPath, Encoding.UTF8).ReadToEnd();
        }
    }
}
