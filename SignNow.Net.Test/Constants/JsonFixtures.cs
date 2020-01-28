using System.IO;

namespace SignNow.Net.Test.Constants
{
    /// <summary>
    /// Provides test fixture names for various SignNow API objects.
    /// </summary>
    public static class JsonFixtures
    {
        /// <summary>
        /// Relative path to Fixtures folder.
        /// </summary>
        /// <returns></returns>
        private static readonly string FixturePath = "../../../TestData/Fixtures".Replace('/', Path.DirectorySeparatorChar);

        /// <summary>
        /// Document base template.
        /// </summary>
        public static readonly string DocumentTemplate = $"{FixturePath}/Document/DocumentResponseTemplate.json";

        /// <summary>
        /// Role base template.
        /// </summary>
        /// <value></value>
        public static readonly string RoleTemplate = $"{FixturePath}/Role/RoleTemplate.json";
    }
}
